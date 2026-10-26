using WebTemplate.DevExpress.Reporting;

namespace WebTemplateApp.Reports;

/// <summary>Bao cao mau: ke thua BaseCompanyReport, khai bao cot, gan DataSource.</summary>
public class SampleListReport : BaseCompanyReport
{
    public SampleListReport() : base("DANH SÁCH MẪU")
    {
        Subtitle = $"Ngày {DateTime.Today:dd/MM/yyyy}";
        AddColumns(
            new ReportColumn("Code", "Mã", 0.8f),
            new ReportColumn("Name", "Tên", 2.5f),
            ReportColumn.Money("Amount", "Giá trị (đ)"));

        DataSource = Enumerable.Range(1, 30)
            .Select(i => new { Code = $"MA{i:000}", Name = $"Mục mẫu {i}", Amount = i * 125_000m })
            .ToList();
    }
}
