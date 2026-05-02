using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Author)
            .ToListAsync();
    }

    public async Task<List<Product>> GetFeaturedProductsAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted && p.IsOnSale)
            .Include(p => p.Category)
            .Include(p => p.Author)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<Product>> GetNewArrivalsAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<Product>> GetMostViewedProductsAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Author)
            .OrderByDescending(p => p.ViewCount)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        return await _context.Products
            .Where(p => !p.IsDeleted && p.Id == id)
            .Include(p => p.Category)
            .Include(p => p.Author)
            .FirstOrDefaultAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await GetProductByIdAsync(id);
        if (product != null)
        {
            product.IsDeleted = true;
            await UpdateProductAsync(product);
        }
    }

    public async Task IncreaseViewCountAsync(Guid id)
    {
        var product = await GetProductByIdAsync(id);
        if (product != null)
        {
            product.ViewCount++;
            await UpdateProductAsync(product);
        }
    }
}