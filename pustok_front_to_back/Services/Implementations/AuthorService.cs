using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Services;

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _context;

    public AuthorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .ToListAsync();
    }

    public async Task<Author> GetAuthorByIdAsync(Guid id)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted && a.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Author> CreateAuthorAsync(Author author)
    {
        if (author == null)
            throw new ArgumentNullException(nameof(author));

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAuthorAsync(Author author)
    {
        if (author == null)
            throw new ArgumentNullException(nameof(author));

        author.UpdatedAt = DateTime.UtcNow;
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task DeleteAuthorAsync(Guid id)
    {
        var author = await GetAuthorByIdAsync(id);
        if (author != null)
        {
            author.IsDeleted = true;
            await UpdateAuthorAsync(author);
        }
    }
}