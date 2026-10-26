using WebTemplate.DevExpress.Reporting;
using WebTemplate.Sample.Data;

namespace WebTemplate.Sample.Reports;

/// <summary>Bao cao dang danh sach: ke thua BaseCompanyReport, khai bao cot va nhom la xong.</summary>
public class CustomerListReport : BaseCompanyReport
{
    public CustomerListReport(SampleDataService data) : base("DANH SÁCH KHÁCH HÀNG")
    {
        Subtitle = $"Tính đến ngày {DateTime.Today:dd/MM/yyyy}";

        AddGroup(nameof(Customer.City), "'Tỉnh/thành: ' + [City]");
        AddColumns(
            new ReportColumn(nameof(Customer.Code), "Mã KH", 0.7f),
            new ReportColumn(nameof(Customer.Name), "Tên khách hàng", 2.4f),
            new ReportColumn(nameof(Customer.Segment), "Nhóm", 1f),
            new ReportColumn(nameof(Customer.Phone), "Điện thoại", 1f),
            new ReportColumn(nameof(Customer.Status), "Trạng thái", 1f),
            ReportColumn.Money(nameof(Customer.Revenue), "Doanh thu (đ)", 1.2f));

        DataSource = data.GetCustomers().OrderBy(c => c.City).ThenBy(c => c.Name).ToList();
    }
}
