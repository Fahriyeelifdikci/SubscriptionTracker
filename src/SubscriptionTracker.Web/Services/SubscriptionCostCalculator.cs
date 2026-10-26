using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.Services;

public static class SubscriptionCostCalculator
{
    public static decimal ToMonthly(decimal price, BillingPeriod period) => period switch
    {
        BillingPeriod.Monthly => price,
        BillingPeriod.Yearly => price / 12m,
        _ => throw new ArgumentOutOfRangeException(nameof(period))
    };

    public static decimal ToYearly(decimal price, BillingPeriod period) => period switch
    {
        BillingPeriod.Monthly => price * 12m,
        BillingPeriod.Yearly => price,
        _ => throw new ArgumentOutOfRangeException(nameof(period))
    };
}