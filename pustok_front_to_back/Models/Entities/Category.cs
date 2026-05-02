namespace pustok_front_to_back.Models.Entities;

public class Category : AuditableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}