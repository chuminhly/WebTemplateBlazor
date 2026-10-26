using DevExpress.XtraReports.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebTemplate.Core;

namespace WebTemplate.DevExpress.Reporting;

/// <summary>Thong tin mot bao cao da dang ky.</summary>
public sealed record ReportEntry(
    string Key,
    string Title,
    string Group,
    string? Description,
    string[]? Roles,
    Func<IServiceProvider, XtraReport> Factory);

/// <summary>
/// Kho bao cao cua ung dung: dang ky mot lan khi khoi dong, trang bao cao tu liet ke va mo theo ma.
/// </summary>
/// <example>
/// <code>
/// builder.Services.AddWebTemplateDevExpress(o =&gt;
/// {
///     o.Reports.Add&lt;DoanhThuReport&gt;("doanh-thu", "Bao cao doanh thu", group: "Ban hang");
/// });
/// </code>
/// </example>
public sealed class ReportCatalogBuilder
{
    internal List<ReportEntry> Entries { get; } = [];

    /// <summary>Dang ky bao cao tao bang constructor rong hoac tu DI (constructor co tham so service).</summary>
    public ReportCatalogBuilder Add<TReport>(string key, string title, string group = "Báo cáo", string? description = null, string[]? roles = null)
        where TReport : XtraReport =>
        Add(key, title, sp => ActivatorUtilities.CreateInstance<TReport>(sp), group, description, roles);

    /// <summary>Dang ky bao cao bang ham tao (vd nap du lieu tu service truoc khi tra ve).</summary>
    public ReportCatalogBuilder Add(string key, string title, Func<IServiceProvider, XtraReport> factory, string group = "Báo cáo", string? description = null, string[]? roles = null)
    {
        Entries.RemoveAll(e => e.Key == key);
        Entries.Add(new ReportEntry(key, title, group, description, roles, factory));
        return this;
    }
}

public interface IReportCatalog
{
    IReadOnlyList<ReportEntry> Entries { get; }

    ReportEntry? Find(string key);

    /// <summary>Tao report moi theo ma; tu dien thong tin cong ty neu report ke thua <see cref="BaseCompanyReport"/>.</summary>
    XtraReport? Create(string key, IServiceProvider services);
}

internal sealed class ReportCatalog(IOptions<WtDevExpressOptions> options, IOptions<WebTemplateOptions> appOptions) : IReportCatalog
{
    public IReadOnlyList<ReportEntry> Entries => options.Value.Reports.Entries;

    public ReportEntry? Find(string key) => Entries.FirstOrDefault(e => string.Equals(e.Key, key, StringComparison.OrdinalIgnoreCase));

    public XtraReport? Create(string key, IServiceProvider services)
    {
        var entry = Find(key);
        if (entry is null)
        {
            return null;
        }
        var report = entry.Factory(services);
        if (report is BaseCompanyReport companyReport)
        {
            companyReport.ApplyCompany(appOptions.Value.Company);
        }
        return report;
    }
}
