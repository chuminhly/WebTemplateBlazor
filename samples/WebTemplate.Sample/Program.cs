using System.Globalization;
using Microsoft.AspNetCore.Components.Routing;
using WebTemplate.Core;
using WebTemplate.Core.Navigation;
using WebTemplate.Core.Theming;
using WebTemplate.DevExpress;
using WebTemplate.Sample.Components;
using WebTemplate.Sample.Data;
using WebTemplate.Sample.Reports;

var builder = WebApplication.CreateBuilder(args);

// Dinh dang so, ngay, tien theo Viet Nam cho toan app (luoi DevExpress cung dung culture nay).
var vi = CultureInfo.GetCultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = vi;
CultureInfo.DefaultThreadCurrentUICulture = vi;

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 1) Giao dien chung: ten app, cong ty, menu, theme.
builder.Services.AddWebTemplate(o =>
{
    o.AppName = "WebTemplate Demo";
    o.FooterText = $"© {DateTime.Now.Year} WebTemplate";
    o.Company = new CompanyInfo
    {
        Name = "Công ty TNHH Phần mềm Mẫu",
        Address = "123 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
        Phone = "028 1234 5678",
        Email = "contact@example.vn",
        TaxCode = "0312345678",
        Website = "www.example.vn",
    };

    o.AddMenu(new MenuItem("Tổng quan", "", "home") { Match = NavLinkMatch.All })
     .AddMenu(MenuItem.Section("Kinh doanh"))
     .AddMenu(new MenuItem("Khách hàng", "khach-hang", "users"))
     .AddMenu(new MenuItem("Đơn hàng", null, "cart")
         .Add(new MenuItem("Danh sách đơn hàng", "don-hang") { Match = NavLinkMatch.All })
         .Add(new MenuItem("Tạo đơn hàng", "don-hang/tao-moi")))
     .AddMenu(new MenuItem("Báo cáo", "bao-cao", "pie"))
     .AddMenu(MenuItem.Section("Hệ thống"))
     .AddMenu(new MenuItem("Giao diện & component", "giao-dien", "palette"))
     .AddMenu(new MenuItem("Trang đăng nhập", "dang-nhap", "lock"));

    // Vi du them theme mau thuong hieu rieng cua khach hang:
    o.AddTheme(new ThemeDefinition("brand", "Brand Navy", "#1e3a8a", "#7aa2ff", "Màu thương hiệu riêng (ví dụ)"));
});

// 2) DevExpress: luoi, report, tieng Viet, kho bao cao.
builder.Services.AddWebTemplateDevExpress(o =>
{
    o.Reports
        .Add<CustomerListReport>("khach-hang", "Danh sách khách hàng theo tỉnh/thành", group: "Khách hàng",
             description: "Nhóm theo tỉnh/thành, có cộng doanh thu từng nhóm")
        .Add<MonthlyRevenueReport>("doanh-thu-thang", "Doanh thu 12 tháng", group: "Bán hàng",
             description: "Bảng và biểu đồ doanh thu, chi phí, lợi nhuận theo tháng")
        .Add("hoa-don-moi-nhat", "Hóa đơn bán hàng (mẫu phiếu)", sp =>
             new InvoiceReport(sp.GetRequiredService<SampleDataService>().GetOrders()[0]), group: "Bán hàng",
             description: "Mẫu phiếu 1 bản ghi: hóa đơn của đơn hàng mới nhất");
});

builder.Services.AddSingleton<SampleDataService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
