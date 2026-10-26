using Microsoft.Extensions.Options;

namespace WebTemplate.Core.Localization;

/// <summary>
/// Chuoi giao dien cua thu vien (tieng Viet / tieng Anh) theo <see cref="WebTemplateOptions.Language"/>.
/// Dung trong component: <c>@inject WtText T</c> roi <c>@T["Save"]</c>.
/// </summary>
public sealed class WtText(IOptions<WebTemplateOptions> options)
{
    private static readonly Dictionary<string, (string Vi, string En)> Strings = new()
    {
        ["Save"] = ("Lưu", "Save"),
        ["Cancel"] = ("Hủy", "Cancel"),
        ["Close"] = ("Đóng", "Close"),
        ["Ok"] = ("Đồng ý", "OK"),
        ["Add"] = ("Thêm mới", "Add"),
        ["Edit"] = ("Sửa", "Edit"),
        ["Delete"] = ("Xóa", "Delete"),
        ["Refresh"] = ("Làm mới", "Refresh"),
        ["Export"] = ("Xuất file", "Export"),
        ["Search"] = ("Tìm kiếm...", "Search..."),
        ["Back"] = ("Quay lại", "Back"),
        ["Home"] = ("Trang chủ", "Home"),
        ["Loading"] = ("Đang tải...", "Loading..."),
        ["NoData"] = ("Chưa có dữ liệu", "No data"),
        ["Notifications"] = ("Thông báo", "Notifications"),
        ["Account"] = ("Tài khoản", "Account"),
        ["Logout"] = ("Đăng xuất", "Sign out"),
        ["ToggleMenu"] = ("Thu gọn / mở rộng menu", "Toggle menu"),
        ["Appearance"] = ("Giao diện", "Appearance"),
        ["ThemeColor"] = ("Màu chủ đạo", "Theme color"),
        ["Mode"] = ("Chế độ", "Mode"),
        ["Light"] = ("Sáng", "Light"),
        ["Dark"] = ("Tối", "Dark"),
        ["System"] = ("Theo máy", "System"),
        ["Density"] = ("Mật độ hiển thị", "Density"),
        ["Comfortable"] = ("Thoáng", "Comfortable"),
        ["Compact"] = ("Gọn", "Compact"),
        ["FontSize"] = ("Cỡ chữ", "Font size"),
        ["Reset"] = ("Khôi phục mặc định", "Reset to default"),
        ["UnsavedChanges"] = ("Bạn có thay đổi chưa lưu. Rời trang và bỏ các thay đổi này?", "You have unsaved changes. Leave and discard them?"),
        ["Error404Title"] = ("Không tìm thấy trang", "Page not found"),
        ["Error404Text"] = ("Trang bạn tìm không tồn tại hoặc đã được chuyển đi.", "The page you are looking for does not exist or was moved."),
        ["Error403Title"] = ("Không có quyền truy cập", "Access denied"),
        ["Error403Text"] = ("Bạn không có quyền xem trang này. Liên hệ quản trị viên nếu cần cấp quyền.", "You do not have permission to view this page."),
        ["Error500Title"] = ("Đã xảy ra lỗi", "Something went wrong"),
        ["Error500Text"] = ("Hệ thống gặp sự cố khi xử lý yêu cầu. Vui lòng thử lại sau.", "An error occurred while processing your request. Please try again."),
        ["Retry"] = ("Thử lại", "Try again"),
        ["GoHome"] = ("Về trang chủ", "Go home"),
    };

    public bool IsVietnamese => !string.Equals(options.Value.Language, "en", StringComparison.OrdinalIgnoreCase);

    public string this[string key] =>
        Strings.TryGetValue(key, out var s) ? (IsVietnamese ? s.Vi : s.En) : key;
}
