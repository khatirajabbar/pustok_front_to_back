namespace pustok_front_to_back.Models.Entities;

public class Slider : AuditableEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [StringLength(300)]
    public string Image { get; set; }

    [StringLength(500)]
    public string ButtonLink { get; set; }

    [StringLength(50)]
    public string ButtonText { get; set; }

    public int Order { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}