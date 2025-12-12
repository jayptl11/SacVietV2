using SacViet.Models;

namespace SacViet.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetMainCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryBySlugAsync(string slug);
    }
}