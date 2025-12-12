using System;
using System.Collections.Generic;

namespace SacViet.Models;

public partial class Otp
{
    public int Otpid { get; set; }

    public int UserId { get; set; }

    public string Otpcode { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
