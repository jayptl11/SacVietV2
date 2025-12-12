namespace SacViet.Services
{
    public interface IWebCrawlerService
    {
        Task<CrawledContentResult> CrawlArticleAsync(string url);
        Task<string> DownloadAndSaveImageAsync(string imageUrl, string articleTitle);
    }

    public class CrawledContentResult
    {
        public bool Success { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public string? SourceUrl { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
