using DevExpress.Blazor;
using WebTemplate.DevExpress.Reporting;

namespace WebTemplate.DevExpress;

/// <summary>Cau hinh phan DevExpress cua WebTemplate.</summary>
public sealed class WtDevExpressOptions
{
    /// <summary>Kich thuoc mac dinh cua control DevExpress. Luoi WtGrid tu chuyen Small khi nguoi dung chon giao dien Gon.</summary>
    public SizeMode SizeMode { get; set; } = SizeMode.Medium;

    /// <summary>Dung ban dich tieng Viet cho chu tren luoi, popup, bo loc (mac dinh bat khi Language = "vi").</summary>
    public bool UseVietnameseLocalization { get; set; } = true;

    /// <summary>Danh sach bao cao cua ung dung.</summary>
    public ReportCatalogBuilder Reports { get; } = new();
}
