using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SacViet.Validation
{
    /// <summary>
    /// Validates that a string is either an absolute HTTP/HTTPS URL or an app-relative path (starting with '/').
    /// Empty/null values are considered valid (use [Required] separately if needed).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UrlOrPathAttribute : ValidationAttribute
    {
        private static readonly Regex AbsoluteHttpRegex = new Regex(
            @"^(https?:)\/\/[^\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var str = value.ToString()?.Trim();
            if (string.IsNullOrEmpty(str))
            {
                return ValidationResult.Success;
            }

            // Accept app-relative paths like /uploads/articles/abc.jpg
            if (str.StartsWith("/"))
            {
                return ValidationResult.Success;
            }

            // Accept absolute http/https URLs
            if (AbsoluteHttpRegex.IsMatch(str))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Giá tr? ph?i là URL h?p l? ho?c ???ng d?n b?t ??u b?ng '/'.");
        }
    }
}
