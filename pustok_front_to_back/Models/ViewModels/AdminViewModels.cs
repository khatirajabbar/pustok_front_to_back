namespace pustok_front_to_back.Models.ViewModels;

/// <summary>
/// Admin dashboard view model
/// </summary>
public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalSliders { get; set; }
    public List<Product> RecentProducts { get; set; } = new();
}

/// <summary>
/// Create product view model
/// </summary>
public class CreateProductViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }
    public int Stock { get; set; }
    public string Sku { get; set; }
    public bool IsOnSale { get; set; }
    
    public List<Category> Categories { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
}

/// <summary>
/// Edit product view model
/// </summary>
public class EditProductViewModel : CreateProductViewModel
{
    public Guid Id { get; set; }
    public string CurrentImage { get; set; }
}