namespace pustok_front_to_back.Models.Entities;

public class Product : AuditableEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [StringLength(300)]
    public string Image { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Guid AuthorId { get; set; }
    public Author Author { get; set; }

    public int ViewCount { get; set; } = 0;
    public bool IsOnSale { get; set; } = false;
    public decimal? SalePrice { get; set; }

    public string Sku { get; set; }
    public int Stock { get; set; } = 0;
}