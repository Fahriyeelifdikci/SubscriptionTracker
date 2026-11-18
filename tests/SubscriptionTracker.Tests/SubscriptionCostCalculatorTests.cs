using SubscriptionTracker.Web.Models;
using SubscriptionTracker.Web.Services;

namespace SubscriptionTracker.Tests;

public class SubscriptionCostCalculatorTests
{
    [Fact]   // tek bir senaryo
    public void ToMonthly_MonthlySubscription_ReturnsSamePrice()
    {
        var result = SubscriptionCostCalculator.ToMonthly(99.99m, BillingPeriod.Monthly);

        Assert.Equal(99.99m, result);
    }

    [Theory]   // aynı test, farklı verilerle birden fazla kez çalışır
    [InlineData(1200, 100)]
    [InlineData(600, 50)]
    [InlineData(0, 0)]
    public void ToMonthly_YearlySubscription_DividesByTwelve(double yearlyPrice, double expected)
    {
        var result = SubscriptionCostCalculator.ToMonthly((decimal)yearlyPrice, BillingPeriod.Yearly);

        Assert.Equal((decimal)expected, result);
    }

    [Fact]
    public void ToYearly_MonthlySubscription_MultipliesByTwelve()
    {
        var result = SubscriptionCostCalculator.ToYearly(59.99m, BillingPeriod.Monthly);

        Assert.Equal(719.88m, result);
    }

    [Fact]
    public void ToMonthly_InvalidPeriod_Throws()
    {
        var invalid = (BillingPeriod)99;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => SubscriptionCostCalculator.ToMonthly(100m, invalid));
    }
}