namespace pustok_front_to_back.Services.Interfaces;

public interface ISliderService
{
    Task<List<Slider>> GetAllSlidersAsync();
    Task<List<Slider>> GetActiveSlidersAsync();
    Task<Slider> GetSliderByIdAsync(Guid id);
    Task<Slider> CreateSliderAsync(Slider slider);
    Task<Slider> UpdateSliderAsync(Slider slider);
    Task DeleteSliderAsync(Guid id);
}