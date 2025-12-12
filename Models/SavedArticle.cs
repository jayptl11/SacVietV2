using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class SavedArticle
{
    public int SaveId { get; set; }

    public int UserId { get; set; }

    public int ArticleId { get; set; }

    public DateTime? SavedAt { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
