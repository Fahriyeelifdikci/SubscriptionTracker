
namespace SubscriptionTracker.Web.ViewModels;

public class DashboardViewModel
{
    public decimal MonthlyTotal { get; set; }
    public decimal YearlyTotal { get; set; }

    public int ActiveCount { get; set; }
    public int TotalCount { get; set; }

    public int ThisMonthPaymentCount { get; set; }
    public decimal ThisMonthPaymentTotal { get; set; }

    public List<UpcomingPaymentViewModel> UpcomingPayments { get; set; } = new();
    public List<CategoryCostViewModel> CategoryCosts { get; set; } = new();

    // Aşama 7: Bütçe sistemi
    public decimal? MonthlyBudget { get; set; }

    public decimal RemainingBudget =>
        MonthlyBudget.HasValue
            ? MonthlyBudget.Value - MonthlyTotal
            : 0;

    public decimal BudgetUsagePercentage =>
        MonthlyBudget > 0
            ? MonthlyTotal / MonthlyBudget.Value * 100
            : 0;
}

public class UpcomingPaymentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly NextPaymentDate { get; set; }
    public int DaysLeft { get; set; }
}

public class CategoryCostViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal MonthlyCost { get; set; }
}
