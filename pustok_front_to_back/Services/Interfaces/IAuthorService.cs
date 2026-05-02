namespace pustok_front_to_back.Services.Interfaces;

public interface IAuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<Author> GetAuthorByIdAsync(Guid id);
    Task<Author> CreateAuthorAsync(Author author);
    Task<Author> UpdateAuthorAsync(Author author);
    Task DeleteAuthorAsync(Guid id);
}