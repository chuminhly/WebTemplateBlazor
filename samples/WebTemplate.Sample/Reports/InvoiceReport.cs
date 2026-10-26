using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using WebTemplate.DevExpress.Reporting;
using WebTemplate.Sample.Data;

namespace WebTemplate.Sample.Reports;

/// <summary>Bao cao dang phieu (1 ban ghi): thong tin hoa don o dau, cac dong hang o giua.</summary>
public class InvoiceReport : BaseCompanyReport
{
    public InvoiceReport(Order order) : base("HÓA ĐƠN BÁN HÀNG")
    {
        Subtitle = $"Số: {order.Code}  ·  Ngày {order.OrderDate:dd} tháng {order.OrderDate:MM} năm {order.OrderDate:yyyy}";
        Signatures = ["Người mua hàng", "Người bán hàng", "Thủ trưởng đơn vị"];

        var top = ReportHeader.HeightF;
        ReportHeader.Controls.AddRange(
        [
            Label("Khách hàng:", 0, top, 110, 20, 9f, DXFontStyle.Bold),
            Label(order.CustomerName, 110, top, ContentWidth - 110, 20),
            Label("Địa chỉ:", 0, top + 20, 110, 20, 9f, DXFontStyle.Bold),
            Label(order.City, 110, top + 20, ContentWidth - 110, 20),
            Label("Trạng thái:", 0, top + 40, 110, 20, 9f, DXFontStyle.Bold),
            Label(order.Status, 110, top + 40, 200, 20),
            Label($"Tổng tiền: {order.Total:#,##0} đ", ContentWidth - 260, top + 40, 260, 20, 10f, DXFontStyle.Bold, TextAlignment.MiddleRight),
        ]);
        ReportHeader.HeightF = top + 72;

        AddColumns(
            new ReportColumn(nameof(OrderLine.ProductName), "Tên hàng hóa, dịch vụ", 2.6f),
            new ReportColumn(nameof(OrderLine.Unit), "ĐVT", 0.6f, Align: TextAlignment.MiddleCenter),
            ReportColumn.Number(nameof(OrderLine.Quantity), "Số lượng", 0.7f),
            ReportColumn.Money(nameof(OrderLine.UnitPrice), "Đơn giá", 1.1f, sum: false),
            ReportColumn.Money(nameof(OrderLine.Amount), "Thành tiền", 1.2f));

        DataSource = order.Lines;
    }
}
