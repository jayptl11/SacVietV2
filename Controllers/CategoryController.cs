using Microsoft.AspNetCore.Mvc;
using SacViet.Repositories;
using SacViet.Services;
using SacViet.ViewModels;

namespace SacViet.Controllers
{
    public class CategoryController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            INewsService newsService, 
            ICategoryRepository categoryRepository,
            IArticleRepository articleRepository,
            ILogger<CategoryController> logger)
        {
            _newsService = newsService;
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
            _logger = logger;
        }

        [Route("category/{categorySlug}")]
        public async Task<IActionResult> Index(string categorySlug, int page = 1)
        {
            try
            {
                // Get category by slug
                var category = await _categoryRepository.GetCategoryBySlugAsync(categorySlug);
                
                if (category == null)
                {
                    return NotFound();
                }

                var (articles, totalCount) = await _newsService.GetArticlesByCategoryAsync(category.CategoryId, page, 12);
                var categories = await _newsService.GetNavigationCategoriesAsync();

                // L?y bài vi?t xem nhi?u nh?t trong chuyên m?c (5 bài)
                var mostViewedArticles = await _articleRepository.GetMostViewedArticlesByCategoryAsync(category.CategoryId, 5);

                var viewModel = new SearchViewModel
                {
                    CategoryId = category.CategoryId,
                    Page = page,
                    PageSize = 12,
                    TotalCount = totalCount,
                    Articles = articles,
                    Categories = categories
                };

                ViewBag.CategoryName = category.CategoryName;
                ViewBag.CategorySlug = categorySlug;
                ViewBag.MostViewedArticles = mostViewedArticles;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category page for {CategorySlug}", categorySlug);
                return NotFound();
            }
        }
    }
}