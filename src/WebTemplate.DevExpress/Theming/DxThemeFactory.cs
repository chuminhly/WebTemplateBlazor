using DevExpress.Blazor;
using WebTemplate.Core.Theming;
using DxThemeMode = DevExpress.Blazor.ThemeMode;

namespace WebTemplate.DevExpress.Theming;

/// <summary>
/// Tao theme DevExpress Fluent khop voi theme WebTemplate: cung che do sang/toi,
/// mau accent = mau chu dao cua theme dang chon.
/// </summary>
public static class DxThemeFactory
{
    public static ITheme Create(ThemeDefinition theme, bool dark) =>
        Themes.Fluent.Clone(p =>
        {
            p.Name = $"wt-{theme.Key}-{(dark ? "dark" : "light")}";
            p.Mode = dark ? DxThemeMode.Dark : DxThemeMode.Light;
            p.SetCustomAccentColor(theme.PrimaryFor(dark));
            // Trang do WebTemplate.Core dinh dang; DevExpress chi lo phan component cua no.
            p.ApplyToPageElements = false;
        });
}
