namespace WebTemplate.Core.Theming;

/// <summary>Luu lua chon giao dien trong localStorage cua trinh duyet (mac dinh).</summary>
public sealed class BrowserThemePreferenceStore(WebTemplateJsInterop js) : IThemePreferenceStore
{
    private const string StorageKey = "wt-theme-preferences";

    public ValueTask<ThemePreferences?> LoadAsync(CancellationToken cancellationToken = default) =>
        js.GetStoredAsync<ThemePreferences>(StorageKey, cancellationToken);

    public ValueTask SaveAsync(ThemePreferences preferences, CancellationToken cancellationToken = default) =>
        js.SetStoredAsync(StorageKey, preferences, cancellationToken);
}
