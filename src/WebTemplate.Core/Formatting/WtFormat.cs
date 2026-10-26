using System.Globalization;

namespace WebTemplate.Core.Formatting;

/// <summary>Dinh dang so, tien, ngay theo chuan Viet Nam.</summary>
public static class WtFormat
{
    public static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

    public const string DateFormat = "dd/MM/yyyy";
    public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
    public const string MoneyFormat = "#,##0 ₫";
    public const string NumberFormat = "#,##0.##";

    public static string Money(decimal? value) => value?.ToString(MoneyFormat, Vi) ?? "";

    /// <summary>Tien rut gon: 1,2 tỷ / 350 tr / 12 N.</summary>
    public static string MoneyShort(decimal value)
    {
        var abs = Math.Abs(value);
        return abs switch
        {
            >= 1_000_000_000 => (value / 1_000_000_000m).ToString("0.#", Vi) + " tỷ",
            >= 1_000_000 => (value / 1_000_000m).ToString("0.#", Vi) + " tr",
            >= 1_000 => (value / 1_000m).ToString("0.#", Vi) + " N",
            _ => value.ToString("0", Vi),
        };
    }

    public static string Number(decimal? value) => value?.ToString(NumberFormat, Vi) ?? "";

    public static string Percent(decimal? value, int decimals = 1) =>
        value is null ? "" : value.Value.ToString("0." + new string('#', decimals), Vi) + "%";

    public static string Date(DateTime? value) => value?.ToString(DateFormat, Vi) ?? "";

    public static string Date(DateOnly? value) => value?.ToString(DateFormat, Vi) ?? "";

    public static string DateTime(DateTime? value) => value?.ToString(DateTimeFormat, Vi) ?? "";
}
