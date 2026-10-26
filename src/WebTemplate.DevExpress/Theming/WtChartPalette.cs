using WebTemplate.Core.Theming;

namespace WebTemplate.DevExpress.Theming;

/// <summary>
/// Bang mau cho bieu do DevExpress: mau dau la mau chu dao cua theme, cac mau sau phan biet ro voi nhau.
/// <code>&lt;DxChart Palette="WtChartPalette.For(Theme)"&gt;</code>
/// </summary>
public static class WtChartPalette
{
    private static readonly string[] Light = ["#94a3b8", "#f59e0b", "#14b8a6", "#e11d48", "#8b5cf6", "#0ea5e9", "#84cc16"];
    private static readonly string[] Dark = ["#64748b", "#fbbf24", "#2dd4bf", "#fb7185", "#a78bfa", "#38bdf8", "#a3e635"];

    public static string[] For(ThemeService theme) => For(theme.CurrentTheme, theme.IsDark);

    public static string[] For(ThemeDefinition theme, bool dark) =>
        [theme.PrimaryFor(dark), .. (dark ? Dark : Light).Where(c => !string.Equals(c, theme.PrimaryFor(dark), StringComparison.OrdinalIgnoreCase))];
}
