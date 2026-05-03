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

        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();
        return slider;
    }

    public async Task<Slider> UpdateSliderAsync(Slider slider)
    {
        if (slider == null)
            throw new ArgumentNullException(nameof(slider));

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
            await UpdateSliderAsync(slider);
        }
    }
}