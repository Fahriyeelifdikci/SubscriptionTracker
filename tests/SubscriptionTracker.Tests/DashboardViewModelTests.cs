using SubscriptionTracker.Web.ViewModels;

namespace SubscriptionTracker.Tests;

public class DashboardViewModelTests
{
    [Fact]
    public void BudgetUsage_SpendingIsNinetyPercent_ReturnsNinety()
    {
        var model = new DashboardViewModel { MonthlyBudget = 1000m, MonthlyTotal = 900m };

        Assert.Equal(90m, model.BudgetUsagePercentage);
        Assert.Equal(100m, model.RemainingBudget);
    }

    [Fact]
    public void BudgetUsage_OverBudget_RemainingIsNegative()
    {
        var model = new DashboardViewModel { MonthlyBudget = 1000m, MonthlyTotal = 1200m };

        Assert.Equal(120m, model.BudgetUsagePercentage);
        Assert.Equal(-200m, model.RemainingBudget);
    }

    [Fact]
    public void BudgetUsage_NoBudgetSet_ReturnsZero()
    {
        var model = new DashboardViewModel { MonthlyBudget = null, MonthlyTotal = 500m };

        Assert.Equal(0m, model.BudgetUsagePercentage);
        Assert.Equal(0m, model.RemainingBudget);
    }

    [Fact]
    public void BudgetUsage_ZeroBudget_DoesNotDivideByZero()
    {
        var model = new DashboardViewModel { MonthlyBudget = 0m, MonthlyTotal = 500m };

        Assert.Equal(0m, model.BudgetUsagePercentage);
    }
}