using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string FullName { get; set; } = null!;

    public string? Avatar { get; set; }

    public int RoleId { get; set; }

    public string? GoogleId { get; set; }

    public bool? IsEmailVerified { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<ArticleApproval> ArticleApprovalReviewedByUsers { get; set; } = new List<ArticleApproval>();

    public virtual ICollection<ArticleApproval> ArticleApprovalSubmittedByUsers { get; set; } = new List<ArticleApproval>();

    public virtual ICollection<ArticleRevision> ArticleRevisions { get; set; } = new List<ArticleRevision>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual ICollection<Otp> Otps { get; set; } = new List<Otp>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SavedArticle> SavedArticles { get; set; } = new List<SavedArticle>();
}
