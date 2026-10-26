using Microsoft.AspNetCore.Http;

namespace WebTemplate.Core.Theming;

/// <summary>
/// Cookie <c>wt-theme</c> do JavaScript ghi moi khi doi theme, de server biet theme
/// ngay tu lan render dau tien (khong bi nhay mau khi tai trang).
/// Dinh dang: <c>themeKey|mode|density|fontScale|resolvedMode</c>, vd <c>ocean|system|comfortable|100|dark</c>.
/// </summary>
public static class ThemeCookie
{
    public const string Name = "wt-theme";

    public static bool TryRead(HttpContext? context, out ThemePreferences preferences, out bool isDark)
    {
        preferences = new ThemePreferences();
        isDark = false;
        var raw = context?.Request.Cookies[Name];
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }

        var parts = Uri.UnescapeDataString(raw).Split('|');
        if (parts.Length < 5)
        {
            return false;
        }

        preferences = new ThemePreferences
        {
            ThemeKey = parts[0],
            Mode = Enum.TryParse<ThemeMode>(parts[1], true, out var mode) ? mode : ThemeMode.Light,
            Density = Enum.TryParse<UiDensity>(parts[2], true, out var density) ? density : UiDensity.Comfortable,
            FontScale = int.TryParse(parts[3], out var scale) ? scale : 100,
        };
        isDark = parts[4] == "dark";
        return true;
    }
}
