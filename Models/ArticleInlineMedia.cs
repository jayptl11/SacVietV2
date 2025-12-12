using System;

namespace SacViet.Models;

public partial class ArticleInlineMedia
{
    public int InlineMediaId { get; set; }
    public int ArticleId { get; set; }
    public string MediaType { get; set; } = null!; // "video" | "image"
    public string Url { get; set; } = null!;
    public string? Title { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? CreatedAt { get; set; }

    public virtual Article Article { get; set; } = null!;
}
