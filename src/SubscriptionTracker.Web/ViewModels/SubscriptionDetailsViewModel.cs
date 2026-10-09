using System.ComponentModel.DataAnnotations;
using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.ViewModels;

public class SubscriptionDetailsViewModel
{
    public int Id { get; set; }

    [Display(Name = "Abonelik Adı")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Kategori")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Ücret")]
    public decimal Price { get; set; }

    [Display(Name = "Ödeme Periyodu")]
    public BillingPeriod BillingPeriod { get; set; }

    [Display(Name = "Başlangıç Tarihi")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "Sonraki Ödeme Tarihi")]
    public DateOnly NextPaymentDate { get; set; }

    [Display(Name = "Durum")]
    public bool IsActive { get; set; }

    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
}