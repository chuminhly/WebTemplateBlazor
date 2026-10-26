using System.Text;

namespace WebTemplate.Core.Theming;

/// <summary>Sinh bien CSS cho tung theme da dang ky (theme dung san + theme du an tu them).</summary>
public static class ThemeCss
{
    public static string Generate(IEnumerable<ThemeDefinition> themes)
    {
        var sb = new StringBuilder();
        foreach (var theme in themes)
        {
            Append(sb, $":root[data-theme=\"{theme.Key}\"]", theme.PrimaryLight);
            Append(sb, $":root[data-theme=\"{theme.Key}\"][data-mode=\"dark\"]", theme.PrimaryDark);
        }
        return sb.ToString();
    }

    private static void Append(StringBuilder sb, string selector, string primary)
    {
        sb.Append(selector)
          .Append("{--wt-primary:").Append(primary)
          .Append(";--wt-primary-contrast:").Append(ThemeDefinition.ContrastText(primary))
          .Append(";}\n");
    }
}
