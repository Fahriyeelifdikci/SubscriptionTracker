using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Data;
using SubscriptionTracker.Web.Extensions;
using SubscriptionTracker.Web.Models;
using SubscriptionTracker.Web.Services;
using SubscriptionTracker.Web.ViewModels;

namespace SubscriptionTracker.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private const int UpcomingDays = 30;

    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();

        // Kullanıcının bütçesini getir
        var budget = await _context.UserBudgets
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserId == userId);

        // Kullanıcının aboneliklerini getir
        var all = await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Price,
                s.BillingPeriod,
                s.NextPaymentDate,
                s.IsActive,
                CategoryName = s.Category.Name
            })
            .ToListAsync();

        var active = all.Where(s => s.IsActive).ToList();

        var today = DateOnly.FromDateTime(DateTime.Today);
        var limit = today.AddDays(UpcomingDays);

        var thisMonth = active
            .Where(s =>
                s.NextPaymentDate.Year == today.Year &&
                s.NextPaymentDate.Month == today.Month)
            .ToList();

        // Dashboard verilerini hazırla
        var model = new DashboardViewModel
        {
            MonthlyBudget = budget?.MonthlyLimit,

            TotalCount = all.Count,
            ActiveCount = active.Count,

            MonthlyTotal = active.Sum(s =>
                SubscriptionCostCalculator.ToMonthly(
                    s.Price, s.BillingPeriod)),

            YearlyTotal = active.Sum(s =>
                SubscriptionCostCalculator.ToYearly(
                    s.Price, s.BillingPeriod)),

            ThisMonthPaymentCount = thisMonth.Count,
            ThisMonthPaymentTotal = thisMonth.Sum(s => s.Price),

            UpcomingPayments = active
                .Where(s =>
                    s.NextPaymentDate >= today &&
                    s.NextPaymentDate <= limit)
                .OrderBy(s => s.NextPaymentDate)
                .Select(s => new UpcomingPaymentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    NextPaymentDate = s.NextPaymentDate,
                    DaysLeft = s.NextPaymentDate.DayNumber -
                               today.DayNumber
                })
                .ToList(),

            CategoryCosts = active
                .GroupBy(s => s.CategoryName)
                .Select(g => new CategoryCostViewModel
                {
                    CategoryName = g.Key,
                    MonthlyCost = g.Sum(s =>
                        SubscriptionCostCalculator.ToMonthly(
                            s.Price, s.BillingPeriod))
                })
                .OrderByDescending(c => c.MonthlyCost)
                .ToList()
        };

        return View(model);
    }

    // Aylık bütçeyi kaydet veya güncelle
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveBudget(decimal monthlyLimit)
    {
        if (monthlyLimit <= 0 || monthlyLimit > 100000000)
        {
            TempData["BudgetError"] =
                "Geçerli bir bütçe tutarı giriniz.";

            return RedirectToAction(nameof(Index));
        }

        var userId = User.GetUserId();

        var budget = await _context.UserBudgets
            .FirstOrDefaultAsync(b => b.UserId == userId);

        if (budget == null)
        {
            // İlk kez bütçe oluşturuluyor
            budget = new UserBudget
            {
                UserId = userId,
                MonthlyLimit = monthlyLimit
            };

            _context.UserBudgets.Add(budget);
        }
        else
        {
            // Mevcut bütçe güncelleniyor
            budget.MonthlyLimit = monthlyLimit;
        }

        await _context.SaveChangesAsync();

        TempData["BudgetSuccess"] =
            "Aylık bütçen başarıyla kaydedildi.";

        return RedirectToAction(nameof(Index));
    }
}
