namespace pustok_front_to_back.Models.ViewModels;

/// <summary>
/// Homepage view model - contains sliders, featured products, new arrivals, and most viewed products
/// </summary>
public class HomeViewModel
{
    public List<Slider> Sliders { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Product> NewArrivals { get; set; } = new();
    public List<Product> MostViewedProducts { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
}

/// <summary>
/// Product detail view model - contains product and related products
/// </summary>
public class ProductDetailViewModel
{
    public Product Product { get; set; }
    public List<Product> RelatedProducts { get; set; } = new();
}

/// <summary>
/// Shop page view model - contains products with pagination and filtering
/// </summary>
public class ShopViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public Guid? CurrentCategory { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

/// <summary>
/// Search results view model
/// </summary>
public class SearchViewModel
{
    public List<Product> Products { get; set; } = new();
    public string SearchTerm { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}