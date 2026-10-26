using System.ComponentModel.DataAnnotations;

namespace WebTemplate.Sample.Data;

public enum CustomerStatus { Active, Potential, Paused }

public class Customer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nhập mã khách hàng")]
    public string Code { get; set; } = "";

    [Required(ErrorMessage = "Nhập tên khách hàng"), StringLength(120)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Chọn tỉnh/thành")]
    public string City { get; set; } = "";

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    public string Segment { get; set; } = "Doanh nghiệp";
    public string Status { get; set; } = "Đang giao dịch";
    public decimal Revenue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Today;
    public string? Note { get; set; }

    public Customer Clone() => (Customer)MemberwiseClone();
}

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "Cái";
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string Code { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Chọn khách hàng")]
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = "";
    public string City { get; set; } = "";

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.Today;

    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; } = "Mới";
    public string? Note { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
    public decimal Total => Lines.Sum(l => l.Amount);
}

public class OrderLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string Unit { get; set; } = "";
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Amount => Quantity * UnitPrice;
}

public record MonthlySales(string Month, DateTime Date, decimal Revenue, decimal Cost)
{
    public decimal Profit => Revenue - Cost;
}
