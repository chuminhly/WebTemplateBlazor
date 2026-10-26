using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebTemplate.Core.Theming;

namespace WebTemplate.Core.Layout;

/// <summary>
/// Thuoc tinh cho the &lt;html&gt; trong App.razor, lay tu cookie theme de lan render dau da dung mau:
/// <code>&lt;html lang="vi" @attributes="WtHtmlAttributes.For(HttpContext, Options.Value)"&gt;</code>
/// </summary>
public static class WtHtmlAttributes
{
    public static Dictionary<string, object> For(HttpContext? context, WebTemplateOptions options)
    {
        var hasCookie = ThemeCookie.TryRead(context, out var p, out var dark);
        if (!hasCookie)
        {
            p = new ThemePreferences { ThemeKey = options.DefaultTheme, Mode = options.DefaultMode, Density = options.DefaultDensity };
            dark = options.DefaultMode == ThemeMode.Dark;
        }
        if (!options.AllowUserThemeSelection || options.Themes.All(t => t.Key != p.ThemeKey))
        {
            p = p with { ThemeKey = options.DefaultTheme };
        }

        return new Dictionary<string, object>
        {
            ["data-theme"] = p.ThemeKey,
            ["data-mode"] = dark ? "dark" : "light",
            ["data-density"] = p.Density == UiDensity.Compact ? "compact" : "comfortable",
            ["style"] = $"--wt-font-scale:{(p.FontScale / 100.0).ToString(System.Globalization.CultureInfo.InvariantCulture)};color-scheme:{(dark ? "dark" : "light")}",
        };
    }

    public static Dictionary<string, object> For(HttpContext? context, IOptions<WebTemplateOptions> options) => For(context, options.Value);
}
