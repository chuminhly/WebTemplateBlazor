namespace WebTemplate.Core.Theming;

/// <summary>10 theme dựng sẵn. Theme đầu tiên là mặc định.</summary>
public static class BuiltInThemes
{
    public static readonly ThemeDefinition Ocean = new("ocean", "Ocean Blue", "#1d6fdc", "#5b9cf5", "Xanh dương - phần mềm doanh nghiệp chung");
    public static readonly ThemeDefinition Emerald = new("emerald", "Emerald", "#0f8a5f", "#34c38f", "Xanh lục bảo - tài chính, nông nghiệp");
    public static readonly ThemeDefinition Purple = new("purple", "Purple", "#7c3aed", "#a78bfa", "Tím - sản phẩm sáng tạo");
    public static readonly ThemeDefinition Sunset = new("sunset", "Sunset Orange", "#e35d0b", "#fb923c", "Cam - bán hàng, bán lẻ");
    public static readonly ThemeDefinition Crimson = new("crimson", "Crimson", "#c62839", "#f2626f", "Đỏ đô - nội bộ, cảnh báo cao");
    public static readonly ThemeDefinition Teal = new("teal", "Teal", "#0d8478", "#2dd4bf", "Xanh ngọc - y tế");
    public static readonly ThemeDefinition Indigo = new("indigo", "Indigo", "#4f46e5", "#818cf8", "Chàm - công nghệ");
    public static readonly ThemeDefinition Rose = new("rose", "Rose", "#d0246f", "#f472b6", "Hồng - thời trang, dịch vụ");
    public static readonly ThemeDefinition Amber = new("amber", "Amber", "#b45309", "#fbbf24", "Vàng hổ phách - kho vận, sản xuất");
    public static readonly ThemeDefinition Slate = new("slate", "Slate", "#475569", "#94a3b8", "Xám than - trung tính, in ấn");

    public static IReadOnlyList<ThemeDefinition> All { get; } =
        [Ocean, Emerald, Purple, Sunset, Crimson, Teal, Indigo, Rose, Amber, Slate];
}
