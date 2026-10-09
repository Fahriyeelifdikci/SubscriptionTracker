using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.ViewModels;

public class SubscriptionListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public BillingPeriod BillingPeriod { get; set; }
    public DateOnly NextPaymentDate { get; set; }
    public bool IsActive { get; set; }
}