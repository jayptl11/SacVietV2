using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int ArticleId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public int? ParentCommentId { get; set; }

    public bool? IsApproved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }

    public virtual User User { get; set; } = null!;
}
