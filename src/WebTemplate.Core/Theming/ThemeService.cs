using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace WebTemplate.Core.Theming;

/// <summary>
/// Quan ly theme cua nguoi dung hien tai (scoped theo circuit).
/// Component nao can phan ung khi doi theme thi dang ky su kien <see cref="Changed"/>.
/// </summary>
public sealed class ThemeService : IAsyncDisposable
{
    private readonly WebTemplateOptions _options;
    private readonly IThemePreferenceStore _store;
    private readonly WebTemplateJsInterop _js;
    private DotNetObjectReference<ThemeService>? _selfRef;
    private bool _initialized;

    public ThemeService(
        IOptions<WebTemplateOptions> options,
        IThemePreferenceStore store,
        WebTemplateJsInterop js,
        IHttpContextAccessor httpContextAccessor)
    {
        _options = options.Value;
        _store = store;
        _js = js;

        // Khi prerender, lay theme tu cookie de HTML dau tien da dung mau.
        if (ThemeCookie.TryRead(httpContextAccessor.HttpContext, out var fromCookie, out var dark))
        {
            Current = fromCookie.Normalize(Themes, _options.DefaultTheme);
            IsDark = dark;
        }
        else
        {
            Current = Defaults;
            IsDark = Current.Mode == ThemeMode.Dark;
        }

        if (!_options.AllowUserThemeSelection)
        {
            Current = Defaults with { Mode = Current.Mode };
        }
    }

    public IReadOnlyList<ThemeDefinition> Themes => _options.Themes;

    public ThemePreferences Current { get; private set; }

    /// <summary>Che do toi dang thuc su ap dung (da tinh ca truong hop theo he dieu hanh).</summary>
    public bool IsDark { get; private set; }

    public ThemeDefinition CurrentTheme => Themes.FirstOrDefault(t => t.Key == Current.ThemeKey) ?? Themes[0];

    public bool AllowUserSelection => _options.AllowUserThemeSelection;

    public event Action? Changed;

    private ThemePreferences Defaults => new()
    {
        ThemeKey = _options.DefaultTheme,
        Mode = _options.DefaultMode,
        Density = _options.DefaultDensity,
    };

    /// <summary>Doc lua chon da luu va ap len trang. Goi sau lan render tuong tac dau tien; goi nhieu lan khong sao.</summary>
    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;

        var saved = _options.AllowUserThemeSelection ? await _store.LoadAsync() : null;
        Current = (saved ?? Current).Normalize(Themes, _options.DefaultTheme);
        _selfRef = DotNetObjectReference.Create(this);
        await _js.WatchSystemModeAsync(_selfRef);
        await ApplyAsync(save: false);
    }

    public Task SetThemeAsync(string themeKey) => UpdateAsync(Current with { ThemeKey = themeKey });

    public Task SetModeAsync(ThemeMode mode) => UpdateAsync(Current with { Mode = mode });

    public Task SetDensityAsync(UiDensity density) => UpdateAsync(Current with { Density = density });

    public Task SetFontScaleAsync(int fontScale) => UpdateAsync(Current with { FontScale = fontScale });

    public Task ResetAsync() => UpdateAsync(Defaults);

    public async Task UpdateAsync(ThemePreferences preferences)
    {
        var next = preferences.Normalize(Themes, _options.DefaultTheme);
        if (!_options.AllowUserThemeSelection)
        {
            next = next with { ThemeKey = _options.DefaultTheme };
        }
        Current = next;
        await ApplyAsync(save: true);
    }

    /// <summary>JavaScript goi khi nguoi dung doi sang/toi trong he dieu hanh.</summary>
    [JSInvokable]
    public async Task OnSystemModeChanged()
    {
        if (Current.Mode == ThemeMode.System)
        {
            await ApplyAsync(save: false);
        }
    }

    private async Task ApplyAsync(bool save)
    {
        IsDark = await _js.ApplyThemeAsync(Current);
        if (save)
        {
            await _store.SaveAsync(Current);
        }
        Changed?.Invoke();
    }

    public ValueTask DisposeAsync()
    {
        _selfRef?.Dispose();
        return ValueTask.CompletedTask;
    }
}
