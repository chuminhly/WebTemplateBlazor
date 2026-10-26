using System.Globalization;

namespace WebTemplate.Core.Theming;

/// <summary>
/// Mot theme mau. Moi theme co mau chu dao cho che do sang va che do toi;
/// cac mau phu (hover, nen nhat, chu tren nen mau) duoc tinh tu dong.
/// </summary>
/// <param name="Key">Ma theme, dung trong thuoc tinh <c>data-theme</c> (chi gom chu thuong, so, dau gach).</param>
/// <param name="Name">Ten hien thi cho nguoi dung.</param>
/// <param name="PrimaryLight">Mau chu dao o che do sang, dang <c>#rrggbb</c>.</param>
/// <param name="PrimaryDark">Mau chu dao o che do toi, dang <c>#rrggbb</c>.</param>
/// <param name="Description">Mo ta ngan, goi y linh vuc phu hop.</param>
public sealed record ThemeDefinition(
    string Key,
    string Name,
    string PrimaryLight,
    string PrimaryDark,
    string? Description = null)
{
    /// <summary>Mau chu dao theo che do dang dung.</summary>
    public string PrimaryFor(bool dark) => dark ? PrimaryDark : PrimaryLight;

    /// <summary>Mau chu (trang hoac gan den) de doc ro tren nen mau chu dao.</summary>
    public static string ContrastText(string hex)
    {
        var (r, g, b) = ParseHex(hex);
        double L(int c)
        {
            var v = c / 255.0;
            return v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        }
        var luminance = 0.2126 * L(r) + 0.7152 * L(g) + 0.0722 * L(b);
        return luminance > 0.4 ? "#0b1220" : "#ffffff";
    }

    internal static (int R, int G, int B) ParseHex(string hex)
    {
        var h = hex.TrimStart('#');
        if (h.Length == 3)
        {
            h = string.Concat(h.Select(c => new string(c, 2)));
        }
        if (h.Length != 6 || !int.TryParse(h, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            throw new FormatException($"Mau '{hex}' khong dung dinh dang #rrggbb.");
        }
        return ((value >> 16) & 0xff, (value >> 8) & 0xff, value & 0xff);
    }
}
