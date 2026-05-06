using Microsoft.AspNetCore.Mvc;
using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ISliderService _sliderService;
    private readonly ICategoryService _categoryService;

    public HomeController(IProductService productService, ISliderService sliderService, ICategoryService categoryService)
    {
        _productService = productService;
        _sliderService = sliderService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _sliderService.GetActiveSlidersAsync();
        var featuredProducts = await _productService.GetFeaturedProductsAsync();
        var newArrivals = await _productService.GetNewArrivalsAsync();
        var mostViewedProducts = await _productService.GetMostViewedProductsAsync();

        var model = new HomeViewModel
        {
            Sliders = sliders,
            FeaturedProducts = featuredProducts,
            NewArrivals = newArrivals,
            MostViewedProducts = mostViewedProducts
        };

        return View(model);
    }

    public async Task<IActionResult> ProductDetail(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        await _productService.IncreaseViewCountAsync(id);

        var relatedProducts = (await _productService.GetAllProductsAsync())
            .Where(p => p.CategoryId == product.CategoryId && p.Id != id)
            .Take(5)
            .ToList();

        var model = new ProductDetailViewModel
        {
            Product = product,
            RelatedProducts = relatedProducts
        };

        return View(model);
    }

    public async Task<IActionResult> Shop(int page = 1, Guid? categoryId = null)
    {
        const int pageSize = 12;

        var allProducts = await _productService.GetAllProductsAsync();

        if (categoryId.HasValue)
            allProducts = allProducts.Where(p => p.CategoryId == categoryId.Value).ToList();

        var categories = await _categoryService.GetAllCategoriesAsync();
        var totalProducts = allProducts.Count;
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        var products = allProducts
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new ShopViewModel
        {
            Products = products,
            Categories = categories,
            CurrentPage = page,
            TotalPages = totalPages,
            SelectedCategoryId = categoryId
        };

        return View(model);
    }

    public async Task<IActionResult> Search(string q, int page = 1)
    {
        const int pageSize = 12;

        var allProducts = await _productService.GetAllProductsAsync();

        if (!string.IsNullOrEmpty(q))
            allProducts = allProducts
                .Where(p => p.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                           p.Description.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var totalProducts = allProducts.Count;
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        var products = allProducts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new SearchViewModel
        {
            Products = products,
            SearchQuery = q,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(model);
    }

    public IActionResult Error()
    {
        return View();
    }
    
}

public class HomeViewModel
{
    public List<Slider> Sliders { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Product> NewArrivals { get; set; } = new();
    public List<Product> MostViewedProducts { get; set; } = new();
}

public class ProductDetailViewModel
{
    public Product Product { get; set; }
    public List<Product> RelatedProducts { get; set; } = new();
}

public class ShopViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public Guid? SelectedCategoryId { get; set; }
}

public class SearchViewModel
{
    public List<Product> Products { get; set; } = new();
    public string SearchQuery { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
