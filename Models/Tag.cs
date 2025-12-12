using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
