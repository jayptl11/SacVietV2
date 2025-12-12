using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class ArticleApproval
{
    public int ApprovalId { get; set; }

    public int ArticleId { get; set; }

    public int SubmittedByUserId { get; set; }

    public int? ReviewedByUserId { get; set; }

    public string Status { get; set; } = null!;

    public string? ReviewNote { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual User? ReviewedByUser { get; set; }

    public virtual User SubmittedByUser { get; set; } = null!;
}
