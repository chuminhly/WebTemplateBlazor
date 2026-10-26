using System.Drawing;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using WebTemplate.DevExpress.Reporting;
using WebTemplate.Sample.Data;

namespace WebTemplate.Sample.Reports;

/// <summary>Bao cao co bieu do: bieu do cot doanh thu/chi phi o dau, bang chi tiet ben duoi.</summary>
public class MonthlyRevenueReport : BaseCompanyReport
{
    public MonthlyRevenueReport(SampleDataService data) : base("BÁO CÁO DOANH THU 12 THÁNG")
    {
        var sales = data.GetMonthlySales();
        Subtitle = $"Từ {sales[0].Date:MM/yyyy} đến {sales[^1].Date:MM/yyyy}";

        var chart = new XRChart
        {
            BoundsF = new RectangleF(0, ReportHeader.HeightF + 4, ContentWidth, 250),
            DataSource = sales,
        };
        chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
        chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
        chart.Legend.Direction = LegendDirection.LeftToRight;
        chart.Series.Add(Series("Doanh thu", nameof(MonthlySales.Revenue), Color.FromArgb(29, 111, 220)));
        chart.Series.Add(Series("Chi phí", nameof(MonthlySales.Cost), Color.FromArgb(148, 163, 184)));
        if (chart.Diagram is XYDiagram diagram)
        {
            diagram.AxisY.Label.TextPattern = "{V:#,##0,,} tr";
        }
        ReportHeader.Controls.Add(chart);
        ReportHeader.HeightF = chart.BottomF + 10;

        AddColumns(
            new ReportColumn(nameof(MonthlySales.Month), "Tháng", 0.8f),
            ReportColumn.Money(nameof(MonthlySales.Revenue), "Doanh thu (đ)"),
            ReportColumn.Money(nameof(MonthlySales.Cost), "Chi phí (đ)"),
            ReportColumn.Money(nameof(MonthlySales.Profit), "Lợi nhuận (đ)"));

        DataSource = sales;
    }

    private static Series Series(string name, string valueField, Color color)
    {
        var series = new Series(name, ViewType.Bar)
        {
            ArgumentDataMember = nameof(MonthlySales.Month),
            LabelsVisibility = global::DevExpress.Utils.DefaultBoolean.False,
        };
        series.ValueDataMembers.AddRange(valueField);
        ((BarSeriesView)series.View).Color = color;
        return series;
    }
}
