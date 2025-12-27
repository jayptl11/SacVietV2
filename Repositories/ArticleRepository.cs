using Microsoft.EntityFrameworkCore;
using SacViet.Models;

namespace SacViet.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly SacVietContext _context;

        public ArticleRepository(SacVietContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetFeaturedArticlesAsync(int count = 5)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now)
                .OrderByDescending(a => a.ViewCount)
                .ThenByDescending(a => a.PublishedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetLatestArticlesAsync(int count = 10)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now)
                .OrderByDescending(a => a.PublishedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetMostViewedTodayAsync(int count = 5)
        {
            // ?u tiên tin g?n nh?t tr??c (theo PublishedAt DESC), sau ?ó m?i ??n ViewCount
            // Th? tìm bài vi?t t? hôm nay và lùi d?n v? các ngày tr??c (t?i ?a 7 ngày)
            for (int daysBack = 0; daysBack <= 7; daysBack++)
            {
                var targetDate = DateTime.Today.AddDays(-daysBack);
                var nextDate = targetDate.AddDays(1);

                var articles = await _context.Articles
                    .Include(a => a.Author)
                    .Include(a => a.Category)
                    .Where(a => a.Status == "Published"
                        && a.PublishedAt <= DateTime.Now
                        && a.PublishedAt >= targetDate
                        && a.PublishedAt < nextDate)
                    .OrderByDescending(a => a.PublishedAt)  // ?u tiên tin m?i nh?t tr??c
                    .ThenByDescending(a => a.ViewCount)     // Sau ?ó m?i ??n view count
                    .Take(count)
                    .ToListAsync();

                // N?u tìm th?y bài vi?t, tr? v? ngay
                if (articles.Any())
                {
                    return articles;
                }
            }

            // N?u không tìm th?y bài vi?t nào trong 7 ngày qua, tr? v? bài vi?t m?i nh?t
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now)
                .OrderByDescending(a => a.PublishedAt)
                .ThenByDescending(a => a.ViewCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetSidebarArticlesAsync(int count = 10, IEnumerable<int> excludeArticleIds = null)
        {
            // ?u tiên tin m?i nh?t tr??c (theo PublishedAt DESC), sau ?ó m?i ??n ViewCount
            var query = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now);

            if (excludeArticleIds != null && excludeArticleIds.Any())
            {
                query = query.Where(a => !excludeArticleIds.Contains(a.ArticleId));
            }

            return await query
                .OrderByDescending(a => a.PublishedAt)  // ?u tiên tin m?i nh?t tr??c
                .ThenByDescending(a => a.ViewCount)     // Sau ?ó m?i ??n view count
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesByCategoryAsync(int categoryId, int page = 1, int pageSize = 10)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.CategoryId == categoryId && a.Status == "Published" && a.PublishedAt <= DateTime.Now)
                .OrderByDescending(a => a.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> SearchArticlesAsync(string keyword, int? categoryId = null, int? authorId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Title.Contains(keyword) || a.Summary.Contains(keyword) || a.Content.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(a => a.AuthorId == authorId.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.PublishedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.PublishedAt <= toDate.Value);
            }

            return await query
                .OrderByDescending(a => a.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.ArticleId == id);
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.Slug == slug);
        }

        public async Task<int> GetTotalArticlesCountAsync()
        {
            return await _context.Articles
                .CountAsync(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now);
        }

        public async Task<int> GetArticlesCountByCategoryAsync(int categoryId)
        {
            return await _context.Articles
                .CountAsync(a => a.CategoryId == categoryId && a.Status == "Published" && a.PublishedAt <= DateTime.Now);
        }

        public async Task<int> GetSearchResultsCountAsync(string keyword, int? categoryId = null, int? authorId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Articles
                .Where(a => a.Status == "Published" && a.PublishedAt <= DateTime.Now);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Title.Contains(keyword) || a.Summary.Contains(keyword) || a.Content.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(a => a.AuthorId == authorId.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.PublishedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.PublishedAt <= toDate.Value);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Article>> GetMostViewedArticlesByCategoryAsync(int categoryId, int count = 5)
        {
            // L?y bài vi?t xem nhi?u nh?t trong 3 ngày g?n ?ây
            var threeDaysAgo = DateTime.Now.AddDays(-3);

            return await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Where(a => a.CategoryId == categoryId
                    && a.Status == "Published"
                    && a.PublishedAt <= DateTime.Now
                    && a.PublishedAt >= threeDaysAgo)
                .OrderByDescending(a => a.ViewCount)
                .ThenByDescending(a => a.PublishedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task IncrementViewCountAsync(int articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article != null)
            {
                article.ViewCount = (article.ViewCount ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Comment>> GetArticleCommentsAsync(int articleId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.InverseParentComment)
                    .ThenInclude(c => c.User)
                .Where(c => c.ArticleId == articleId && c.IsApproved == true && c.ParentCommentId == null)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment?> AddCommentAsync(int articleId, int userId, string content, int? parentCommentId = null)
        {
            var comment = new Comment
            {
                ArticleId = articleId,
                UserId = userId,
                Content = content,
                ParentCommentId = parentCommentId,
                IsApproved = true, // Auto-approve for now, you can change this logic
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == comment.CommentId);
        }

        public async Task<int> GetCommentCountAsync(int articleId)
        {
            return await _context.Comments
                .CountAsync(c => c.ArticleId == articleId && c.IsApproved == true);
        }
    }
}