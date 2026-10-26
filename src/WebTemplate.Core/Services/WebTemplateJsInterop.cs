using System.Text.Json;
using Microsoft.JSInterop;
using WebTemplate.Core.Theming;

namespace WebTemplate.Core;

/// <summary>Cau noi JavaScript cua thu vien (module <c>_content/WebTemplate.Core/js/webtemplate.js</c>).</summary>
public sealed class WebTemplateJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private Task<IJSObjectReference>? _module;

    private Task<IJSObjectReference> Module =>
        _module ??= jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/WebTemplate.Core/js/webtemplate.js").AsTask();

    /// <summary>Ap theme len the &lt;html&gt;, ghi cookie; tra ve true neu dang o che do toi.</summary>
    public async ValueTask<bool> ApplyThemeAsync(ThemePreferences preferences)
    {
        var module = await Module;
        return await module.InvokeAsync<bool>("applyTheme", new
        {
            theme = preferences.ThemeKey,
            mode = preferences.Mode.ToString().ToLowerInvariant(),
            density = preferences.Density.ToString().ToLowerInvariant(),
            fontScale = preferences.FontScale,
        });
    }

    public async ValueTask WatchSystemModeAsync<T>(DotNetObjectReference<T> target) where T : class
    {
        var module = await Module;
        await module.InvokeVoidAsync("watchSystemMode", target);
    }

    public async ValueTask<T?> GetStoredAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var module = await Module;
        var json = await module.InvokeAsync<string?>("getStored", cancellationToken, key);
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }
        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public async ValueTask SetStoredAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        var module = await Module;
        await module.InvokeVoidAsync("setStored", cancellationToken, key, JsonSerializer.Serialize(value));
    }

    public async ValueTask RemoveStoredAsync(string key)
    {
        var module = await Module;
        await module.InvokeVoidAsync("removeStored", key);
    }

    public async ValueTask ScrollToTopAsync()
    {
        var module = await Module;
        await module.InvokeVoidAsync("scrollToTop");
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is { IsCompletedSuccessfully: true })
        {
            try
            {
                await _module.Result.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Circuit da dong, khong can giai phong phia trinh duyet.
            }
        }
    }
}
