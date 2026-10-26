using System.Drawing;
using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using WebTemplate.Core;

namespace WebTemplate.DevExpress.Reporting;

/// <summary>Mot cot trong bang cua bao cao dang danh sach.</summary>
/// <param name="Field">Ten thuoc tinh du lieu.</param>
/// <param name="Caption">Tieu de cot.</param>
/// <param name="Weight">Ty le do rong so voi cac cot khac.</param>
/// <param name="Format">Chuoi dinh dang, vd <c>{0:#,##0}</c> hoac <c>{0:dd/MM/yyyy}</c>.</param>
/// <param name="Align">Canh le.</param>
/// <param name="Sum">Cong tong cot nay o cuoi nhom va cuoi bao cao.</param>
public sealed record ReportColumn(
    string Field,
    string Caption,
    float Weight = 1,
    string? Format = null,
    TextAlignment Align = TextAlignment.MiddleLeft,
    bool Sum = false)
{
    public static ReportColumn Money(string field, string caption, float weight = 1.2f, bool sum = true) =>
        new(field, caption, weight, "{0:#,##0}", TextAlignment.MiddleRight, sum);

    public static ReportColumn Number(string field, string caption, float weight = 0.8f, bool sum = false) =>
        new(field, caption, weight, "{0:#,##0.##}", TextAlignment.MiddleRight, sum);

    public static ReportColumn Date(string field, string caption, float weight = 0.9f) =>
        new(field, caption, weight, "{0:dd/MM/yyyy}", TextAlignment.MiddleCenter);
}

/// <summary>
/// Report goc co san mau in chuan cong ty: logo + ten + dia chi, tieu de, ngay in, so trang, cho ky ten.
/// Report moi ke thua lop nay roi goi <see cref="AddColumns"/> (bao cao danh sach) hoac tu them control vao
/// <see cref="Detail"/>, <see cref="ReportHeader"/>.
/// </summary>
public class BaseCompanyReport : XtraReport
{
    protected static readonly string FontFamily = "Arial";
    protected static readonly Color HeaderBack = Color.FromArgb(235, 240, 248);
    protected static readonly Color GridLineColor = Color.FromArgb(190, 198, 210);

    private readonly XRLabel _companyName;
    private readonly XRLabel _companyInfo;
    private readonly XRPictureBox _logo;
    private readonly XRLabel _title;
    private readonly XRLabel _subtitle;
    private readonly XRLabel _footerLeft;
    private readonly XRLabel _pageCompany;
    private readonly XRLabel _pageTitle;
    private readonly List<ReportColumn> _sumColumns = [];
    private readonly List<GroupFooterBand> _groupFooters = [];
    private ReportColumn[] _columns = [];
    private bool _finalized;

    public BaseCompanyReport() : this("Báo cáo") { }

    public BaseCompanyReport(string title, bool landscape = false)
    {
        PaperKind = DXPaperKind.A4;
        Landscape = landscape;
        Margins = new DXMargins(50, 50, 40, 40);
        Font = new DXFont(FontFamily, 9f);

        TopMargin = new TopMarginBand { HeightF = 40 };
        BottomMargin = new BottomMarginBand { HeightF = 40 };
        // Trang dau: khoi thong tin cong ty + tieu de nam trong ReportHeader.
        // Cac trang sau: PageHeader gon (ten cong ty + ten bao cao).
        ReportHeader = new ReportHeaderBand { HeightF = 130 };
        PageHeader = new PageHeaderBand { HeightF = 30, PrintOn = PrintOnPages.NotWithReportHeader };
        Detail = new DetailBand { HeightF = 22 };
        ReportFooter = new ReportFooterBand { HeightF = 10 };
        Bands.AddRange([TopMargin, ReportHeader, PageHeader, Detail, ReportFooter, BottomMargin]);

        // ---- Thong tin cong ty + ngay in ----
        _logo = new XRPictureBox { BoundsF = new RectangleF(0, 0, 48, 48), Sizing = global::DevExpress.XtraPrinting.ImageSizeMode.ZoomImage, Visible = false };
        _companyName = Label("", 0, 0, 420, 20, 11f, DXFontStyle.Bold);
        _companyInfo = Label("", 0, 20, 420, 32, 8f);
        _companyInfo.Multiline = true;
        _companyInfo.ForeColor = Color.FromArgb(90, 100, 115);
        var printed = new XRPageInfo
        {
            PageInfo = PageInfo.DateTime,
            TextFormatString = "Ngày in: {0:dd/MM/yyyy HH:mm}",
            BoundsF = new RectangleF(ContentWidth - 220, 0, 220, 18),
            TextAlignment = TextAlignment.TopRight,
            Font = new DXFont(FontFamily, 8f),
            ForeColor = Color.FromArgb(90, 100, 115),
        };
        var line = new XRLine { BoundsF = new RectangleF(0, 56, ContentWidth, 4), ForeColor = GridLineColor, LineWidth = 1 };

        // ---- Tieu de bao cao ----
        _title = Label(title, 0, 68, ContentWidth, 28, 15f, DXFontStyle.Bold, TextAlignment.MiddleCenter);
        _subtitle = Label("", 0, 98, ContentWidth, 20, 9f, DXFontStyle.Italic, TextAlignment.MiddleCenter);
        ReportHeader.Controls.AddRange([_logo, _companyName, _companyInfo, printed, line, _title, _subtitle]);

        // ---- Dau cac trang sau ----
        _pageCompany = Label("", 0, 0, ContentWidth / 2, 20, 8f, DXFontStyle.Bold);
        _pageTitle = Label(title, ContentWidth / 2, 0, ContentWidth / 2, 20, 8f, DXFontStyle.Regular, TextAlignment.MiddleRight);
        _pageTitle.ForeColor = _pageCompany.ForeColor = Color.FromArgb(90, 100, 115);
        PageHeader.Controls.AddRange([_pageCompany, _pageTitle,
            new XRLine { BoundsF = new RectangleF(0, 22, ContentWidth, 4), ForeColor = GridLineColor, LineWidth = 1 }]);

        // ---- Chan trang: nguon + so trang ----
        _footerLeft = Label("", 0, 10, 400, 18, 7.5f);
        _footerLeft.ForeColor = Color.Gray;
        var pageNumber = new XRPageInfo
        {
            PageInfo = PageInfo.NumberOfTotal,
            TextFormatString = "Trang {0}/{1}",
            BoundsF = new RectangleF(ContentWidth - 150, 10, 150, 18),
            TextAlignment = TextAlignment.MiddleRight,
            Font = new DXFont(FontFamily, 7.5f),
            ForeColor = Color.Gray,
        };
        BottomMargin.Controls.AddRange([_footerLeft, pageNumber]);
    }

    public TopMarginBand TopMargin { get; }
    public BottomMarginBand BottomMargin { get; }
    public PageHeaderBand PageHeader { get; }
    public ReportHeaderBand ReportHeader { get; }
    public DetailBand Detail { get; }
    public ReportFooterBand ReportFooter { get; }

    /// <summary>Chieu rong vung in (da tru le trai/phai).</summary>
    public float ContentWidth => (Landscape ? 1169 : 827) - Margins.Left - Margins.Right;

    public string ReportTitle
    {
        get => _title.Text;
        set => _title.Text = _pageTitle.Text = value;
    }

    /// <summary>Dong phu duoi tieu de, vd "Tu ngay 01/10/2026 den 31/10/2026".</summary>
    public string? Subtitle
    {
        get => _subtitle.Text;
        set => _subtitle.Text = value ?? "";
    }

    /// <summary>Cac o ky ten cuoi bao cao. Mang rong = khong in phan ky ten.</summary>
    public string[] Signatures { get; set; } = ["Người lập biểu", "Kế toán trưởng", "Giám đốc"];

    /// <summary>Do WebTemplate goi tu dong (qua IReportCatalog) de dien thong tin cong ty tu cau hinh.</summary>
    public void ApplyCompany(CompanyInfo company)
    {
        _companyName.Text = company.Name.ToUpperInvariant();
        _pageCompany.Text = company.Name;
        var lines = new[]
        {
            company.Address,
            string.Join("  ·  ", new[] { company.Phone is null ? null : "ĐT: " + company.Phone, company.Email, company.TaxCode is null ? null : "MST: " + company.TaxCode }.Where(s => !string.IsNullOrEmpty(s))),
        }.Where(s => !string.IsNullOrEmpty(s));
        _companyInfo.Text = string.Join(Environment.NewLine, lines);
        _footerLeft.Text = company.Website ?? company.Name;

        if (company.LogoFilePath is { } logo && File.Exists(logo))
        {
            _logo.ImageSource = new global::DevExpress.XtraPrinting.Drawing.ImageSource(DXImage.FromStream(File.OpenRead(logo)));
            _logo.Visible = true;
            _companyName.LeftF = _companyInfo.LeftF = 58;
        }
    }

    /// <summary>Tao bang danh sach: dong tieu de cot lap lai moi trang, dong du lieu, dong tong (cot co Sum = true).</summary>
    protected void AddColumns(params ReportColumn[] columns)
    {
        _columns = columns;
        var header = new GroupHeaderBand { HeightF = 26, RepeatEveryPage = true, Level = 0 };
        Bands.Add(header);
        header.Controls.Add(BuildRow(columns, c => c.Caption, isHeader: true));
        Detail.Controls.Add(BuildRow(columns, c => null, isHeader: false));
        _sumColumns.AddRange(columns.Where(c => c.Sum));
    }

    /// <summary>Nhom du lieu theo mot truong, co dong tieu de nhom va dong cong tong nhom.</summary>
    /// <param name="field">Truong de nhom.</param>
    /// <param name="captionExpression">Bieu thuc tieu de nhom, vd <c>'Thanh pho: ' + [City]</c>.</param>
    protected void AddGroup(string field, string captionExpression)
    {
        var level = _groupFooters.Count + 1;
        var groupHeader = new GroupHeaderBand { HeightF = 24, Level = level };
        groupHeader.GroupFields.Add(new GroupField(field));
        var caption = Label("", 0, 2, ContentWidth, 20, 9.5f, DXFontStyle.Bold);
        caption.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", captionExpression));
        caption.ForeColor = Color.FromArgb(29, 78, 160);
        groupHeader.Controls.Add(caption);

        var groupFooter = new GroupFooterBand { HeightF = 24, Level = level };
        Bands.AddRange([groupHeader, groupFooter]);
        _groupFooters.Add(groupFooter);
    }

    protected override void BeforeReportPrint()
    {
        if (!_finalized)
        {
            _finalized = true;
            BuildTotals();
            BuildSignatures();
        }
        base.BeforeReportPrint();
    }

    private void BuildTotals()
    {
        if (_sumColumns.Count == 0)
        {
            return;
        }
        foreach (var footer in _groupFooters)
        {
            footer.Controls.Add(BuildTotalRow(SummaryRunning.Group, "Cộng nhóm"));
        }
        var total = BuildTotalRow(SummaryRunning.Report, "TỔNG CỘNG");
        total.TopF = ReportFooter.HeightF;
        ReportFooter.Controls.Add(total);
        ReportFooter.HeightF += total.HeightF + 6;
    }

    private void BuildSignatures()
    {
        if (Signatures.Length == 0)
        {
            return;
        }
        var top = ReportFooter.HeightF + 16;
        var width = ContentWidth / Signatures.Length;
        var date = Label($"Ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}", ContentWidth - width, top, width, 18, 9f, DXFontStyle.Italic, TextAlignment.MiddleCenter);
        ReportFooter.Controls.Add(date);
        for (var i = 0; i < Signatures.Length; i++)
        {
            ReportFooter.Controls.Add(Label(Signatures[i], i * width, top + 20, width, 18, 9.5f, DXFontStyle.Bold, TextAlignment.MiddleCenter));
            var hint = Label("(Ký, ghi rõ họ tên)", i * width, top + 38, width, 16, 8f, DXFontStyle.Italic, TextAlignment.MiddleCenter);
            hint.ForeColor = Color.Gray;
            ReportFooter.Controls.Add(hint);
        }
        ReportFooter.HeightF = top + 120;
    }

    private XRTable BuildTotalRow(SummaryRunning running, string caption)
    {
        var table = BuildRow(_columns, _ => null, isHeader: false);
        var row = table.Rows[0];
        var captionPlaced = false;
        for (var i = 0; i < _columns.Length; i++)
        {
            var cell = row.Cells[i];
            cell.ExpressionBindings.Clear();
            cell.Font = new DXFont(FontFamily, 9f, DXFontStyle.Bold);
            cell.BackColor = Color.FromArgb(246, 248, 251);
            var column = _columns[i];
            if (column.Sum)
            {
                cell.Summary = new XRSummary { Running = running };
                cell.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", $"sumSum([{column.Field}])"));
            }
            else if (!captionPlaced)
            {
                cell.Text = caption;
                captionPlaced = true;
            }
        }
        return table;
    }

    private XRTable BuildRow(ReportColumn[] columns, Func<ReportColumn, string?> text, bool isHeader)
    {
        var table = new XRTable
        {
            BoundsF = new RectangleF(0, 0, ContentWidth, isHeader ? 26 : 22),
            Borders = BorderSide.All,
            BorderColor = GridLineColor,
            Font = new DXFont(FontFamily, isHeader ? 9f : 8.5f, isHeader ? DXFontStyle.Bold : DXFontStyle.Regular),
        };
        table.BeginInit();
        var row = new XRTableRow();
        var totalWeight = columns.Sum(c => c.Weight);
        foreach (var column in columns)
        {
            var cell = new XRTableCell
            {
                WidthF = ContentWidth * column.Weight / totalWeight,
                Padding = new PaddingInfo(5, 5, 2, 2),
                TextAlignment = isHeader ? TextAlignment.MiddleCenter : column.Align,
                WordWrap = isHeader,
            };
            if (isHeader)
            {
                cell.Text = text(column);
                cell.BackColor = HeaderBack;
            }
            else
            {
                cell.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", $"[{column.Field}]"));
                if (column.Format is not null)
                {
                    cell.TextFormatString = column.Format;
                }
            }
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
        table.EndInit();
        return table;
    }

    /// <summary>Tao nhan nhanh voi font cua bao cao.</summary>
    protected static XRLabel Label(string text, float x, float y, float width, float height, float size = 9f,
        DXFontStyle style = DXFontStyle.Regular, TextAlignment align = TextAlignment.MiddleLeft) => new()
    {
        Text = text,
        BoundsF = new RectangleF(x, y, width, height),
        Font = new DXFont(FontFamily, size, style),
        TextAlignment = align,
        Padding = new PaddingInfo(2, 2, 0, 0),
        CanGrow = true,
    };
}
