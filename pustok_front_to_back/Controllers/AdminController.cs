using Microsoft.AspNetCore.Mvc;
using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Controllers;

public class AdminController : Controller
{
    private readonly IProductService _productService;
    private readonly IAuthorService _authorService;
    private readonly ISliderService _sliderService;
    private readonly ICategoryService _categoryService;
    private readonly IFileUploadService _fileUploadService;

    public AdminController(
        IProductService productService,
        IAuthorService authorService,
        ISliderService sliderService,
        ICategoryService categoryService,
        IFileUploadService fileUploadService)
    {
        _productService = productService;
        _authorService = authorService;
        _sliderService = sliderService;
        _categoryService = categoryService;
        _fileUploadService = fileUploadService;
    }

    // DASHBOARD
    public async Task<IActionResult> Dashboard()
    {
        var totalProducts = (await _productService.GetAllProductsAsync()).Count;
        var totalAuthors = (await _authorService.GetAllAuthorsAsync()).Count;
        var totalCategories = (await _categoryService.GetAllCategoriesAsync()).Count;
        var totalSliders = (await _sliderService.GetAllSlidersAsync()).Count;

        var model = new AdminDashboardViewModel
        {
            TotalProducts = totalProducts,
            TotalAuthors = totalAuthors,
            TotalCategories = totalCategories,
            TotalSliders = totalSliders,
            RecentProducts = (await _productService.GetAllProductsAsync()).OrderByDescending(p => p.CreatedAt).Take(5).ToList()
        };

        return View(model);
    }

    // ============ AUTHORS CRUD ============

    public async Task<IActionResult> Authors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return View(authors);
    }

    public IActionResult CreateAuthor()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor(Author author, IFormFile profileImage)
    {
        if (!ModelState.IsValid)
            return View(author);

        try
        {
            if (profileImage != null)
                author.ProfileImage = await _fileUploadService.UploadImageAsync(profileImage, "authors");

            await _authorService.CreateAuthorAsync(author);
            TempData["Success"] = "Author created successfully!";
            return RedirectToAction(nameof(Authors));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            return View(author);
        }
    }

    public async Task<IActionResult> EditAuthor(Guid id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        if (author == null)
            return NotFound();
        return View(author);
    }

    [HttpPost]
    public async Task<IActionResult> EditAuthor(Author author, IFormFile profileImage)
    {
        if (!ModelState.IsValid)
            return View(author);

        try
        {
            var existingAuthor = await _authorService.GetAuthorByIdAsync(author.Id);
            if (existingAuthor == null)
                return NotFound();

            if (profileImage != null)
            {
                if (!string.IsNullOrEmpty(existingAuthor.ProfileImage))
                    await _fileUploadService.DeleteImageAsync(existingAuthor.ProfileImage);

                author.ProfileImage = await _fileUploadService.UploadImageAsync(profileImage, "authors");
            }
            else
            {
                author.ProfileImage = existingAuthor.ProfileImage;
            }

            await _authorService.UpdateAuthorAsync(author);
            TempData["Success"] = "Author updated successfully!";
            return RedirectToAction(nameof(Authors));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            return View(author);
        }
    }

    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        try
        {
            await _authorService.DeleteAuthorAsync(id);
            TempData["Success"] = "Author deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
        }

        return RedirectToAction(nameof(Authors));
    }

    // ============ SLIDERS CRUD ============

    public async Task<IActionResult> Sliders()
    {
        var sliders = await _sliderService.GetAllSlidersAsync();
        return View(sliders);
    }

    public IActionResult CreateSlider()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateSlider(Slider slider, IFormFile image)
    {
        if (!ModelState.IsValid)
            return View(slider);

        try
        {
            if (image != null)
                slider.Image = await _fileUploadService.UploadImageAsync(image, "sliders");

            await _sliderService.CreateSliderAsync(slider);
            TempData["Success"] = "Slider created successfully!";
            return RedirectToAction(nameof(Sliders));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            return View(slider);
        }
    }

    public async Task<IActionResult> EditSlider(Guid id)
    {
        var slider = await _sliderService.GetSliderByIdAsync(id);
        if (slider == null)
            return NotFound();
        return View(slider);
    }

    [HttpPost]
    public async Task<IActionResult> EditSlider(Slider slider, IFormFile image)
    {
        if (!ModelState.IsValid)
            return View(slider);

        try
        {
            var existingSlider = await _sliderService.GetSliderByIdAsync(slider.Id);
            if (existingSlider == null)
                return NotFound();

            if (image != null)
            {
                if (!string.IsNullOrEmpty(existingSlider.Image))
                    await _fileUploadService.DeleteImageAsync(existingSlider.Image);

                slider.Image = await _fileUploadService.UploadImageAsync(image, "sliders");
            }
            else
            {
                slider.Image = existingSlider.Image;
            }

            await _sliderService.UpdateSliderAsync(slider);
            TempData["Success"] = "Slider updated successfully!";
            return RedirectToAction(nameof(Sliders));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            return View(slider);
        }
    }

    public async Task<IActionResult> DeleteSlider(Guid id)
    {
        try
        {
            await _sliderService.DeleteSliderAsync(id);
            TempData["Success"] = "Slider deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
        }

        return RedirectToAction(nameof(Sliders));
    }

    // ============ PRODUCTS CRUD ============

    public async Task<IActionResult> Products()
    {
        var products = await _productService.GetAllProductsAsync();
        return View(products);
    }

    public async Task<IActionResult> CreateProduct()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var authors = await _authorService.GetAllAuthorsAsync();

        var model = new CreateProductViewModel
        {
            Categories = categories,
            Authors = authors
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel model, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _categoryService.GetAllCategoriesAsync();
            model.Authors = await _authorService.GetAllAuthorsAsync();
            return View(model);
        }

        try
        {
            var product = new Product
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                IsOnSale = model.IsOnSale,
                SalePrice = model.SalePrice,
                Sku = model.Sku,
                Stock = model.Stock
            };

            if (image != null)
                product.Image = await _fileUploadService.UploadImageAsync(image, "products");

            await _productService.CreateProductAsync(product);
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Products));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            model.Categories = await _categoryService.GetAllCategoriesAsync();
            model.Authors = await _authorService.GetAllAuthorsAsync();
            return View(model);
        }
    }

    public async Task<IActionResult> EditProduct(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        var categories = await _categoryService.GetAllCategoriesAsync();
        var authors = await _authorService.GetAllAuthorsAsync();

        var model = new CreateProductViewModel
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            AuthorId = product.AuthorId,
            IsOnSale = product.IsOnSale,
            SalePrice = product.SalePrice,
            Sku = product.Sku,
            Stock = product.Stock,
            Image = product.Image,
            Categories = categories,
            Authors = authors
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(CreateProductViewModel model, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _categoryService.GetAllCategoriesAsync();
            model.Authors = await _authorService.GetAllAuthorsAsync();
            return View(model);
        }

        try
        {
            var product = await _productService.GetProductByIdAsync(model.Id);
            if (product == null)
                return NotFound();

            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;
            product.AuthorId = model.AuthorId;
            product.IsOnSale = model.IsOnSale;
            product.SalePrice = model.SalePrice;
            product.Sku = model.Sku;
            product.Stock = model.Stock;

            if (image != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                    await _fileUploadService.DeleteImageAsync(product.Image);

                product.Image = await _fileUploadService.UploadImageAsync(image, "products");
            }

            await _productService.UpdateProductAsync(product);
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Products));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            model.Categories = await _categoryService.GetAllCategoriesAsync();
            model.Authors = await _authorService.GetAllAuthorsAsync();
            return View(model);
        }
    }

    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            TempData["Success"] = "Product deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
        }

        return RedirectToAction(nameof(Products));
    }
}

// VIEW MODELS
public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSliders { get; set; }
    public List<Product> RecentProducts { get; set; } = new();
}

public class CreateProductViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsOnSale { get; set; }
    public decimal? SalePrice { get; set; }
    public string Sku { get; set; }
    public int Stock { get; set; }
    public string Image { get; set; }
    public List<Category> Categories { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
}
