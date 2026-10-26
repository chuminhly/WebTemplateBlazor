namespace WebTemplate.Core.Theming;

public enum ThemeMode
{
    Light,
    Dark,
    /// <summary>Theo cai dat sang/toi cua he dieu hanh.</summary>
    System
}

public enum UiDensity
{
    /// <summary>Thoang, de doc - mac dinh.</summary>
    Comfortable,
    /// <summary>Gon, hien nhieu du lieu tren man hinh.</summary>
    Compact
}

/// <summary>Lua chon giao dien cua mot nguoi dung.</summary>
public sealed record ThemePreferences
{
    public string ThemeKey { get; init; } = BuiltInThemes.Ocean.Key;
    public ThemeMode Mode { get; init; } = ThemeMode.Light;
    public UiDensity Density { get; init; } = UiDensity.Comfortable;

    /// <summary>Co chu theo phan tram: 90, 100, 110 hoac 120.</summary>
    public int FontScale { get; init; } = 100;

    public static readonly int[] FontScales = [90, 100, 110, 120];

    internal ThemePreferences Normalize(IReadOnlyCollection<ThemeDefinition> themes, string fallbackKey) => this with
    {
        ThemeKey = themes.Any(t => t.Key == ThemeKey) ? ThemeKey : fallbackKey,
        FontScale = FontScales.Contains(FontScale) ? FontScale : 100,
    };
}
