using Microsoft.AspNetCore.Identity;

namespace SubscriptionTracker.Web.Models;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } //para için decimal kullandım.
    public BillingPeriod BillingPeriod { get; set; }
    public DateOnly StartDate { get; set; } //ödeme tarihlerinde saat bilgisine ihtiyacımız yok, sadece gün önemli.
    public DateOnly NextPaymentDate { get; set; } //bir sonraki ödeme tarihi de sadece gün bazında önemli.
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } //bu alanın isteğe bağlı olduğunu gösterir. EF Core bunu veritabanında NULL kabul eden sütun yapar.

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string UserId { get; set; } = string.Empty; //UserId bir string, çünkü IdentityUser'ın birincil anahtarı varsayılan olarak string
    public IdentityUser User { get; set; } = null!;
}