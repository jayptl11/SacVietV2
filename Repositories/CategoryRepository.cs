using Microsoft.EntityFrameworkCore;
using SacViet.Models;

namespace SacViet.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SacVietContext _context;

        public CategoryRepository(SacVietContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null && c.IsActive == true)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == parentCategoryId && c.IsActive == true)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.InverseParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.InverseParentCategory)
                .FirstOrDefaultAsync(c => c.Slug == slug);
        }
    }
}