using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SacViet.Services;
using SacViet.ViewModels;
using System.Security.Claims;

namespace SacViet.Controllers
{
    [Authorize(Roles = "Writer,Admin")]
    public class WriterController : Controller
    {
        private readonly IWriterService _writerService;
        private readonly INewsService _newsService;
        private readonly IWebCrawlerService _crawlerService;

        public WriterController(IWriterService writerService, INewsService newsService, IWebCrawlerService crawlerService)
        {
            _writerService = writerService;
            _newsService = newsService;
            _crawlerService = crawlerService;
        }

        // Dashboard Index
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var stats = await _writerService.GetWriterStatsAsync(userId);
            return View(stats);
        }

        // Danh sách bài viết của writer
        public async Task<IActionResult> MyArticles(string? status = null, int page = 1)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var (articles, totalCount) = await _writerService.GetMyArticlesAsync(userId, status, page, 10);
            
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / 10.0);
            
            return View(articles);
        }

        // Tạo bài viết mới
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
            return View(new ArticleCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleCreateViewModel model, [FromForm] string? submitAction)
        {
            // VALIDATE THUMBNAIL for Submit action
            if (submitAction == "submit" && string.IsNullOrWhiteSpace(model.ThumbnailImage))
            {
                ModelState.AddModelError("ThumbnailImage", "Vui lòng chọn ảnh đại diện trước khi gửi duyệt");
                ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
                
                // Set TempData for client-side alert
                TempData["ErrorMessage"] = "⚠️ Vui lòng chọn ảnh đại diện trước khi gửi duyệt!";
                
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
                return View(model);
            }

            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Xác định status dựa trên button
                string status = submitAction switch
                {
                    "draft" => "Draft",
                    "submit" => "Pending",
                    _ => "Draft"
                };

                var articleId = await _writerService.CreateArticleAsync(model, userId, status);

                if (articleId > 0)
                {
                    TempData["SuccessMessage"] = status == "Draft" 
                        ? "Bài viết đã được lưu nháp thành công!" 
                        : "Bài viết đã được gửi để chờ duyệt!";
                    
                    return RedirectToAction("Edit", new { id = articleId });
                }
                else
                {
                    ModelState.AddModelError("", "Không thể tạo bài viết. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            }

            ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
            return View(model);
        }

        // Chỉnh sửa bài viết
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var article = await _writerService.GetArticleForEditAsync(id, userId);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy bài viết hoặc bạn không có quyền chỉnh sửa.";
                return RedirectToAction("MyArticles");
            }

            ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
            
            // Get approval status if exists
            var approval = await _writerService.GetLatestApprovalAsync(id);
            ViewBag.ApprovalStatus = approval;

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleEditViewModel model, [FromForm] string? submitAction)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            // VALIDATE THUMBNAIL for Submit action
            if (submitAction == "submit" && string.IsNullOrWhiteSpace(model.ThumbnailImage))
            {
                ModelState.AddModelError("ThumbnailImage", "Vui lòng chọn ảnh đại diện trước khi gửi duyệt");
                
                // IMPORTANT: Restore article data from DB to preserve all fields
                var articleData = await _writerService.GetArticleForEditAsync(id, userId);
                if (articleData != null)
                {
                    // Preserve user input for editable fields
                    articleData.Title = model.Title ?? articleData.Title;
                    articleData.Summary = model.Summary ?? articleData.Summary;
                    articleData.Content = model.Content ?? articleData.Content;
                    articleData.ThumbnailImage = model.ThumbnailImage ?? articleData.ThumbnailImage;
                    articleData.ScheduledPublishDate = model.ScheduledPublishDate ?? articleData.ScheduledPublishDate;
                    articleData.Tags = model.Tags ?? articleData.Tags;
                    
                    // Restore CategoryId if not provided
                    if (model.CategoryId == 0)
                    {
                        articleData.CategoryId = articleData.CategoryId; // Keep original
                    }
                    else
                    {
                        articleData.CategoryId = model.CategoryId; // Use user input
                    }
                    
                    model = articleData; // Replace model with restored data
                }
                
                ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
                var approval = await _writerService.GetLatestApprovalAsync(id);
                ViewBag.ApprovalStatus = approval;
                
                TempData["ErrorMessage"] = "⚠️ Vui lòng chọn ảnh đại diện trước khi gửi duyệt!";
                
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                // IMPORTANT: Restore article data from DB
                var articleData = await _writerService.GetArticleForEditAsync(id, userId);
                if (articleData != null)
                {
                    // Preserve user input for editable fields
                    articleData.Title = model.Title ?? articleData.Title;
                    articleData.Summary = model.Summary ?? articleData.Summary;
                    articleData.Content = model.Content ?? articleData.Content;
                    articleData.ThumbnailImage = model.ThumbnailImage ?? articleData.ThumbnailImage;
                    articleData.ScheduledPublishDate = model.ScheduledPublishDate ?? articleData.ScheduledPublishDate;
                    articleData.Tags = model.Tags ?? articleData.Tags;
                    
                    // Restore CategoryId if not provided
                    if (model.CategoryId == 0)
                    {
                        articleData.CategoryId = articleData.CategoryId; // Keep original
                    }
                    else
                    {
                        articleData.CategoryId = model.CategoryId; // Use user input
                    }
                    
                    model = articleData; // Replace model with restored data
                }
                
                ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
                var approval = await _writerService.GetLatestApprovalAsync(id);
                ViewBag.ApprovalStatus = approval;
                
                return View(model);
            }

            try
            {
                // Xác định status dựa trên button
                string status = submitAction switch
                {
                    "draft" => "Draft",
                    "submit" => "Pending",
                    _ => model.Status ?? "Draft"
                };

                var success = await _writerService.UpdateArticleAsync(id, model, userId, status);

                if (success)
                {
                    TempData["SuccessMessage"] = status == "Draft"
                        ? "Bài viết đã được lưu nháp!"
                        : "Bài viết đã được gửi để chờ duyệt!";
                    
                    return RedirectToAction("Edit", new { id });
                }
                else
                {
                    ModelState.AddModelError("", "Không thể cập nhật bài viết.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            }

            // IMPORTANT: Restore article data on any error
            var articleDataFinal = await _writerService.GetArticleForEditAsync(id, userId);
            if (articleDataFinal != null)
            {
                // Preserve user input for editable fields
                articleDataFinal.Title = model.Title ?? articleDataFinal.Title;
                articleDataFinal.Summary = model.Summary ?? articleDataFinal.Summary;
                articleDataFinal.Content = model.Content ?? articleDataFinal.Content;
                articleDataFinal.ThumbnailImage = model.ThumbnailImage ?? articleDataFinal.ThumbnailImage;
                articleDataFinal.ScheduledPublishDate = model.ScheduledPublishDate ?? articleDataFinal.ScheduledPublishDate;
                articleDataFinal.Tags = model.Tags ?? articleDataFinal.Tags;
                
                // Restore CategoryId if not provided
                if (model.CategoryId == 0)
                {
                    articleDataFinal.CategoryId = articleDataFinal.CategoryId; // Keep original
                }
                else
                {
                    articleDataFinal.CategoryId = model.CategoryId; // Use user input
                }
                
                model = articleDataFinal; // Replace model with restored data
            }
            
            ViewBag.Categories = await _newsService.GetAllCategoriesAsync();
            var approvalFinal = await _writerService.GetLatestApprovalAsync(id);
            ViewBag.ApprovalStatus = approvalFinal;
            
            return View(model);
        }

        // Xóa bài viết (chỉ được xóa Draft)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập." });
            }

            try
            {
                var success = await _writerService.DeleteArticleAsync(id, userId);
                
                if (success)
                {
                    return Json(new { success = true, message = "Xóa bài viết thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không thể xóa bài viết. Chỉ được xóa bài ở trạng thái nháp." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Xem chi tiết trạng thái duyệt
        public async Task<IActionResult> ApprovalHistory(int articleId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var article = await _writerService.GetArticleForEditAsync(articleId, userId);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy bài viết.";
                return RedirectToAction("MyArticles");
            }

            var history = await _writerService.GetApprovalHistoryAsync(articleId);
            ViewBag.Article = article;
            
            return View(history);
        }

        // Upload image for article
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn file ảnh." });
            }

            try
            {
                var imageUrl = await _writerService.UploadImageAsync(file);
                return Json(new { success = true, url = imageUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi upload: {ex.Message}" });
            }
        }

        // Crawl content from URL
        [HttpPost]
        public async Task<IActionResult> CrawlUrl([FromBody] CrawlUrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                return Json(new { success = false, message = "Vui lòng nhập URL" });
            }

            try
            {
                var result = await _crawlerService.CrawlArticleAsync(request.Url);
                
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.ErrorMessage ?? "Không thể crawl nội dung" });
                }

                // Use thumbnail from meta tags, or first image in content as fallback
                string? thumbnailUrl = result.ThumbnailUrl;
                
                // If no thumbnail from meta tags, try to get first image from content
                if (string.IsNullOrWhiteSpace(thumbnailUrl) && result.ImageUrls.Any())
                {
                    thumbnailUrl = result.ImageUrls.First();
                }

                // Try to download thumbnail to local server (optional, fallback to original URL)
                string? localThumbnail = thumbnailUrl;
                if (!string.IsNullOrWhiteSpace(thumbnailUrl))
                {
                    try
                    {
                        localThumbnail = await _crawlerService.DownloadAndSaveImageAsync(thumbnailUrl, result.Title ?? "article");
                    }
                    catch
                    {
                        // If download fails, use original URL
                        localThumbnail = thumbnailUrl;
                    }
                }

                // Return content with original image URLs embedded
                return Json(new
                {
                    success = true,
                    title = result.Title,
                    summary = result.Summary,
                    content = result.Content,
                    thumbnailUrl = localThumbnail,
                    sourceUrl = result.SourceUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Set thumbnail by URL
        [HttpPost]
        public async Task<IActionResult> SetThumbnailByUrl([FromBody] SetThumbnailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ImageUrl))
            {
                return Json(new { success = false, message = "Vui lòng nhập URL hình ảnh" });
            }

            try
            {
                // Validate URL
                if (!Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out _))
                {
                    return Json(new { success = false, message = "URL không hợp lệ" });
                }

                // Option 1: Return URL as-is (fast, no download)
                if (!request.DownloadToServer)
                {
                    return Json(new { success = true, url = request.ImageUrl });
                }

                // Option 2: Download to server
                try
                {
                    var localUrl = await _crawlerService.DownloadAndSaveImageAsync(request.ImageUrl, "thumbnail");
                    return Json(new { success = true, url = localUrl });
                }
                catch (Exception ex)
                {
                    // If download fails, fallback to original URL
                    return Json(new { success = true, url = request.ImageUrl, warning = $"Không tải về được, dùng URL gốc. Lỗi: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }
    }

    public class CrawlUrlRequest
    {
        public string Url { get; set; } = string.Empty;
    }

    public class SetThumbnailRequest
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool DownloadToServer { get; set; } = true;
    }
}
