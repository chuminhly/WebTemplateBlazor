namespace WebTemplate.Core.Theming;

/// <summary>
/// Noi luu lua chon giao dien. Mac dinh luu trong trinh duyet (localStorage + cookie).
/// Du an co the thay bang ban luu vao CSDL theo tai khoan:
/// <c>services.AddScoped&lt;IThemePreferenceStore, MyDbThemeStore&gt;()</c> sau <c>AddWebTemplate()</c>.
/// </summary>
public interface IThemePreferenceStore
{
    /// <summary>Tra ve lua chon da luu, hoac null neu nguoi dung chua chon.</summary>
    ValueTask<ThemePreferences?> LoadAsync(CancellationToken cancellationToken = default);

    ValueTask SaveAsync(ThemePreferences preferences, CancellationToken cancellationToken = default);
}
