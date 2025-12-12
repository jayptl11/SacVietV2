using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class ArticleMedium
{
    public int ArticleMediaId { get; set; }

    public int ArticleId { get; set; }

    public int MediaId { get; set; }

    public string MediaType { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public string? Caption { get; set; }

    public string? AltText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual Medium Media { get; set; } = null!;
}
