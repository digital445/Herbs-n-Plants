using Plants.Models.Dto;

namespace Plants.Services.IServices
{
    public interface IPlantsService : IBaseService
    {
        Task<T?> GetPageAsync<T>(int page, int pageSize);
        Task<T?> GetFilteredAsync<T>(FilterDto filter, int page, int pageSize);
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateUpdateAsync<T>(PlantDto plantDto);
        Task<T?> DeleteAsync<T>(int id);
        Task<T?> GetPaletteAsync<T>();
	}
}
