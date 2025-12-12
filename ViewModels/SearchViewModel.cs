using SacViet.Models;

namespace SacViet.ViewModels
{
    public class SearchViewModel
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        public IEnumerable<Article> Articles { get; set; } = new List<Article>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}