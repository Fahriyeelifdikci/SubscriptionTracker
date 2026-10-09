using System.ComponentModel.DataAnnotations;

namespace SubscriptionTracker.Web.Models;

public enum BillingPeriod
{
    [Display(Name = "Aylık")]
    Monthly = 1,

    [Display(Name = "Yıllık")]
    Yearly = 2
}