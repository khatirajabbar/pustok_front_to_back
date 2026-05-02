using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Services;

public class SliderService : ISliderService
{
    private readonly AppDbContext _context;

    public SliderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Slider>> GetAllSlidersAsync()
    {
        return await _context.Sliders
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<List<Slider>> GetActiveSlidersAsync()
    {
        return await _context.Sliders
            .Where(s => !s.IsDeleted && s.IsActive)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<Slider> GetSliderByIdAsync(Guid id)
    {
        return await _context.Sliders
            .Where(s => !s.IsDeleted && s.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Slider> CreateSliderAsync(Slider slider)
    {
        if (slider == null)
            throw new ArgumentNullException(nameof(slider));

        if (string.IsNullOrWhiteSpace(slider.Title))
            throw new ArgumentException("Slider title is required");

        if (string.IsNullOrWhiteSpace(slider.Image))
            throw new ArgumentException("Slider image is required");

        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();
        return slider;
    }

    public async Task<Slider> UpdateSliderAsync(Slider slider)
    {
        if (slider == null)
            throw new ArgumentNullException(nameof(slider));

        var existingSlider = await GetSliderByIdAsync(slider.Id);
        if (existingSlider == null)
            throw new InvalidOperationException($"Slider with ID {slider.Id} not found");

        slider.UpdatedAt = DateTime.UtcNow;
        _context.Sliders.Update(slider);
        await _context.SaveChangesAsync();
        return slider;
    }

    public async Task DeleteSliderAsync(Guid id)
    {
        var slider = await GetSliderByIdAsync(id);
        if (slider != null)
        {
            slider.IsDeleted = true;
            slider.UpdatedAt = DateTime.UtcNow;
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
        }
    }
}
