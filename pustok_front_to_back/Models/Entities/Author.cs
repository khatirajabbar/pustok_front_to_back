namespace pustok_front_to_back.Models.Entities;

public class Author : AuditableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [StringLength(1000)]
    public string Bio { get; set; }

    [StringLength(300)]
    public string ProfileImage { get; set; } // Path to image

    public ICollection<Product> Products { get; set; } = new List<Product>();
}