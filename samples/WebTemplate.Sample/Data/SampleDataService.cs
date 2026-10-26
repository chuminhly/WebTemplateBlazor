namespace WebTemplate.Sample.Data;

/// <summary>Du lieu gia lap trong bo nho de trinh dien. Du an that thay bang service goi CSDL/API.</summary>
public class SampleDataService
{
    public static readonly string[] Cities = ["Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "Bình Dương", "Đồng Nai", "Khánh Hòa"];
    public static readonly string[] Segments = ["Doanh nghiệp", "Đại lý", "Bán lẻ", "Cá nhân"];
    public static readonly string[] CustomerStatuses = ["Đang giao dịch", "Tiềm năng", "Tạm dừng"];
    public static readonly string[] OrderStatuses = ["Mới", "Đang xử lý", "Đang giao", "Hoàn thành", "Đã hủy"];

    public static readonly IReadOnlyDictionary<string, string> CustomerStatusTones = new Dictionary<string, string>
    {
        ["Đang giao dịch"] = "success", ["Tiềm năng"] = "info", ["Tạm dừng"] = "warning",
    };

    public static readonly IReadOnlyDictionary<string, string> OrderStatusTones = new Dictionary<string, string>
    {
        ["Mới"] = "primary", ["Đang xử lý"] = "info", ["Đang giao"] = "warning", ["Hoàn thành"] = "success", ["Đã hủy"] = "danger",
    };

    private static readonly string[] Prefixes = ["Công ty TNHH", "Công ty CP", "Cửa hàng", "Tập đoàn", "Nhà phân phối"];
    private static readonly string[] Names = ["An Phát", "Minh Long", "Hòa Bình", "Thành Công", "Phú Thịnh", "Sao Việt", "Đại Dương", "Hưng Thịnh", "Tân Tiến", "Kim Ngân", "Bảo Châu", "Trường Sơn", "Việt Hưng", "Hồng Hà", "Gia Phát", "Nam Á", "Đông Dương", "Thái Bình", "Vạn Xuân", "Phương Nam"];
    private static readonly (string Name, string Unit, decimal Price)[] Products =
    [
        ("Máy in laser A4", "Cái", 4_590_000), ("Giấy A4 Double A", "Ram", 78_000), ("Mực in HP 85A", "Hộp", 1_250_000),
        ("Bàn làm việc 1m2", "Cái", 2_350_000), ("Ghế xoay văn phòng", "Cái", 1_690_000), ("Màn hình 24 inch", "Cái", 3_290_000),
        ("Bàn phím không dây", "Bộ", 650_000), ("Router WiFi 6", "Cái", 1_890_000), ("Ổ cứng SSD 1TB", "Cái", 2_150_000),
        ("Laptop văn phòng 14\"", "Cái", 16_900_000), ("Tủ hồ sơ 4 ngăn", "Cái", 3_850_000), ("Máy chiếu Full HD", "Cái", 12_400_000),
    ];

    private readonly List<Customer> _customers = [];
    private readonly List<Order> _orders = [];
    private readonly List<Product> _products = [];
    private readonly Lock _lock = new();

    public SampleDataService()
    {
        var rnd = new Random(42);
        for (var i = 0; i < Products.Length; i++)
        {
            _products.Add(new Product { Id = i + 1, Code = $"SP{i + 1:000}", Name = Products[i].Name, Unit = Products[i].Unit, Price = Products[i].Price });
        }

        for (var i = 1; i <= 48; i++)
        {
            _customers.Add(new Customer
            {
                Id = i,
                Code = $"KH{i:0000}",
                Name = $"{Prefixes[rnd.Next(Prefixes.Length)]} {Names[rnd.Next(Names.Length)]} {(i % 3 == 0 ? "Group" : "")}".Trim(),
                City = Cities[rnd.Next(Cities.Length)],
                Phone = $"09{rnd.Next(10_000_000, 99_999_999)}",
                Email = $"lienhe{i}@example.vn",
                Segment = Segments[rnd.Next(Segments.Length)],
                Status = CustomerStatuses[rnd.Next(10) switch { < 7 => 0, < 9 => 1, _ => 2 }],
                CreatedAt = DateTime.Today.AddDays(-rnd.Next(20, 900)),
            });
        }

        for (var i = 1; i <= 160; i++)
        {
            var c = _customers[rnd.Next(_customers.Count)];
            var date = DateTime.Today.AddDays(-rnd.Next(0, 365));
            var order = new Order
            {
                Id = i,
                Code = $"DH{date:yyMM}-{i:0000}",
                CustomerId = c.Id,
                CustomerName = c.Name,
                City = c.City,
                OrderDate = date,
                DeliveryDate = date.AddDays(rnd.Next(1, 7)),
                Status = date > DateTime.Today.AddDays(-7) ? OrderStatuses[rnd.Next(0, 3)] : (rnd.Next(12) == 0 ? "Đã hủy" : "Hoàn thành"),
            };
            for (var l = 0; l < rnd.Next(1, 5); l++)
            {
                var p = _products[rnd.Next(_products.Count)];
                order.Lines.Add(new OrderLine { ProductId = p.Id, ProductName = p.Name, Unit = p.Unit, Quantity = rnd.Next(1, 12), UnitPrice = p.Price });
            }
            _orders.Add(order);
        }

        foreach (var c in _customers)
        {
            c.Revenue = _orders.Where(o => o.CustomerId == c.Id && o.Status != "Đã hủy").Sum(o => o.Total);
        }
    }

    public IReadOnlyList<Customer> GetCustomers() { lock (_lock) return [.. _customers]; }

    public Customer? GetCustomer(int id) { lock (_lock) return _customers.FirstOrDefault(c => c.Id == id); }

    public IReadOnlyList<Product> GetProducts() => _products;

    public IReadOnlyList<Order> GetOrders() { lock (_lock) return [.. _orders.OrderByDescending(o => o.OrderDate)]; }

    public Order? GetOrder(int id) { lock (_lock) return _orders.FirstOrDefault(o => o.Id == id); }

    public IReadOnlyList<Order> GetOrdersOf(int customerId) => GetOrders().Where(o => o.CustomerId == customerId).ToList();

    public void SaveCustomer(Customer customer)
    {
        lock (_lock)
        {
            if (customer.Id == 0)
            {
                customer.Id = _customers.Max(c => c.Id) + 1;
                _customers.Insert(0, customer);
                return;
            }
            var index = _customers.FindIndex(c => c.Id == customer.Id);
            _customers[index] = customer;
        }
    }

    public void DeleteCustomers(IEnumerable<int> ids)
    {
        lock (_lock)
        {
            var set = ids.ToHashSet();
            _customers.RemoveAll(c => set.Contains(c.Id));
        }
    }

    public void SaveOrder(Order order)
    {
        lock (_lock)
        {
            order.CustomerName = _customers.FirstOrDefault(c => c.Id == order.CustomerId)?.Name ?? "";
            if (order.Id == 0)
            {
                order.Id = _orders.Max(o => o.Id) + 1;
                order.Code = $"DH{order.OrderDate:yyMM}-{order.Id:0000}";
                _orders.Add(order);
                return;
            }
            var index = _orders.FindIndex(o => o.Id == order.Id);
            _orders[index] = order;
        }
    }

    public void DeleteOrders(IEnumerable<int> ids)
    {
        lock (_lock)
        {
            var set = ids.ToHashSet();
            _orders.RemoveAll(o => set.Contains(o.Id));
        }
    }

    public IReadOnlyList<MonthlySales> GetMonthlySales()
    {
        var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-11);
        return Enumerable.Range(0, 12).Select(i =>
        {
            var month = start.AddMonths(i);
            var revenue = GetOrders().Where(o => o.Status != "Đã hủy" && o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month).Sum(o => o.Total);
            return new MonthlySales($"T{month.Month}/{month:yy}", month, revenue, Math.Round(revenue * (0.64m + (i * 7 % 5) * 0.035m), 0));
        }).ToList();
    }
}
