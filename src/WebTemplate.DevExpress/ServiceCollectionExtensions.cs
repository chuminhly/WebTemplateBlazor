using DevExpress.Blazor.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using WebTemplate.Core;
using WebTemplate.DevExpress.Localization;
using WebTemplate.DevExpress.Reporting;

namespace WebTemplate.DevExpress;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Dang ky DevExpress Blazor, trinh xem bao cao, ban dich tieng Viet va kho bao cao.
    /// Goi sau <c>AddWebTemplate()</c>.
    /// </summary>
    public static IServiceCollection AddWebTemplateDevExpress(this IServiceCollection services, Action<WtDevExpressOptions>? configure = null)
    {
        var options = new WtDevExpressOptions();
        configure?.Invoke(options);
        services.AddSingleton(Options.Create(options));

        services.AddDevExpressBlazor(o => o.SizeMode = options.SizeMode);
        services.AddDevExpressServerSideBlazorReportViewer();

        services.TryAddSingleton<IReportCatalog, ReportCatalog>();

        if (options.UseVietnameseLocalization)
        {
            services.AddSingleton<IDxLocalizationService>(sp =>
                new WtDxLocalizationService(sp.GetRequiredService<IOptions<WebTemplateOptions>>()));
        }
        return services;
    }
}
