using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class Medium
{
    public int MediaId { get; set; }

    public string FileName { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string? MimeType { get; set; }

    public long FileSize { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public int? Duration { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int UploadedByUserId { get; set; }

    public string? StoragePath { get; set; }

    public bool? IsPublic { get; set; }

    public int? UsageCount { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<ArticleMedium> ArticleMedia { get; set; } = new List<ArticleMedium>();

    public virtual User UploadedByUser { get; set; } = null!;
}
