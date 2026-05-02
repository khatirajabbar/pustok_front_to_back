namespace pustok_front_to_back.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<List<Product>> GetFeaturedProductsAsync();
    Task<List<Product>> GetNewArrivalsAsync();
    Task<List<Product>> GetMostViewedProductsAsync();
    Task<Product> GetProductByIdAsync(Guid id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(Guid id);
    Task IncreaseViewCountAsync(Guid id);
}