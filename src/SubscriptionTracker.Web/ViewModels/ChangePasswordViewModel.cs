
using System.ComponentModel.DataAnnotations;

namespace SubscriptionTracker.Web.ViewModels;

public class ChangePasswordViewModel
{
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mevcut şifrenizi giriniz.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifrenizi giriniz.")]
    [StringLength(
        100,
        MinimumLength = 8,
        ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifrenizi tekrar giriniz.")]
    [Compare(
        nameof(NewPassword),
        ErrorMessage = "Şifreler eşleşmiyor.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
