using WebTemplate.Core.Navigation;
using WebTemplate.Core.Theming;

namespace WebTemplate.Core;

/// <summary>Cau hinh chung cho ung dung dung WebTemplate. Khai bao trong <c>AddWebTemplate(o =&gt; ...)</c>.</summary>
public sealed class WebTemplateOptions
{
    /// <summary>Ten ung dung hien tren thanh tren cung va tieu de tab.</summary>
    public string AppName { get; set; } = "WebTemplate";

    /// <summary>Duong dan logo (tuy chon). Neu bo trong se hien chu cai dau cua ten app.</summary>
    public string? LogoUrl { get; set; }

    public string FooterText { get; set; } = $"© {DateTime.Now.Year}";

    /// <summary>Danh sach menu ben trai.</summary>
    public List<MenuItem> Menu { get; } = [];

    /// <summary>Thong tin cong ty, dung cho header bao cao va footer.</summary>
    public CompanyInfo Company { get; set; } = new();

    /// <summary>Tat ca theme nguoi dung duoc chon. Mac dinh co 10 theme dung san.</summary>
    public List<ThemeDefinition> Themes { get; } = [.. BuiltInThemes.All];

    public string DefaultTheme { get; set; } = BuiltInThemes.Ocean.Key;

    public ThemeMode DefaultMode { get; set; } = ThemeMode.Light;

    public UiDensity DefaultDensity { get; set; } = UiDensity.Comfortable;

    /// <summary>Tat de khoa theme mac dinh cho toan he thong (an bo chon theme, van cho doi sang/toi).</summary>
    public bool AllowUserThemeSelection { get; set; } = true;

    /// <summary>Ngon ngu giao dien: "vi" (mac dinh) hoac "en".</summary>
    public string Language { get; set; } = "vi";

    /// <summary>Them theme rieng cua du an, vd mau thuong hieu khach hang.</summary>
    public WebTemplateOptions AddTheme(ThemeDefinition theme)
    {
        Themes.RemoveAll(t => t.Key == theme.Key);
        Themes.Add(theme);
        return this;
    }

    /// <summary>Them mot muc menu cap 1.</summary>
    public WebTemplateOptions AddMenu(MenuItem item)
    {
        Menu.Add(item);
        return this;
    }
}

/// <summary>Thong tin cong ty dung cho bao cao, chan trang.</summary>
public sealed class CompanyInfo
{
    public string Name { get; set; } = "Công ty của bạn";
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string? Website { get; set; }
    /// <summary>Duong dan file logo tren o dia (dung cho report), vd <c>wwwroot/images/logo.png</c>.</summary>
    public string? LogoFilePath { get; set; }
}
