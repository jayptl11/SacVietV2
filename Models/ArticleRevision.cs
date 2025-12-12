using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class ArticleRevision
{
    public int RevisionId { get; set; }

    public int ArticleId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int EditedByUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual User EditedByUser { get; set; } = null!;
}
