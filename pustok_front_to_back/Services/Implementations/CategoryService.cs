using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted && c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        category.UpdatedAt = DateTime.UtcNow;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await GetCategoryByIdAsync(id);
        if (category != null)
        {
            category.IsDeleted = true;
            await UpdateCategoryAsync(category);
        }
    }
}