using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.ViewModels;

public class SubscriptionFormViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Abonelik adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Abonelik adı en fazla 100 karakter olabilir.")]
    [Display(Name = "Abonelik Adı")]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçiniz.")]
    [Display(Name = "Kategori")]
    public int CategoryId { get; set; }

    [Range(0.01, 1000000, ErrorMessage = "Ücret 0,01 ile 1.000.000 arasında olmalıdır.")]
    [Display(Name = "Ücret (TL)")]
    public decimal Price { get; set; }

    [Display(Name = "Ödeme Periyodu")]
    public BillingPeriod BillingPeriod { get; set; } = BillingPeriod.Monthly;

    [Display(Name = "Başlangıç Tarihi")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "Sonraki Ödeme Tarihi")]
    public DateOnly NextPaymentDate { get; set; }

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NextPaymentDate < StartDate)
        {
            yield return new ValidationResult(
                "Sonraki ödeme tarihi başlangıç tarihinden önce olamaz.",
                new[] { nameof(NextPaymentDate) });
        }
    }
}