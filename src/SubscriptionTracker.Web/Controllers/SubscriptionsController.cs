using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Data;
using SubscriptionTracker.Web.Models;
using SubscriptionTracker.Web.ViewModels;

namespace SubscriptionTracker.Web.Controllers;

[Authorize]
public class SubscriptionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SubscriptionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index()
    {
        var userId = CurrentUserId;

        var subscriptions = await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderBy(s => s.NextPaymentDate)
            .Select(s => new SubscriptionListItemViewModel
            {
                Id = s.Id,
                Name = s.Name,
                CategoryName = s.Category.Name,
                Price = s.Price,
                BillingPeriod = s.BillingPeriod,
                NextPaymentDate = s.NextPaymentDate,
                IsActive = s.IsActive
            })
            .ToListAsync();

        return View(subscriptions);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var model = new SubscriptionFormViewModel
        {
            StartDate = today,
            NextPaymentDate = today.AddMonths(1)
        };

        await PopulateCategoriesAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionFormViewModel model)
    {
        if (ModelState.IsValid && !await _context.Categories.AnyAsync(c => c.Id == model.CategoryId))
        {
            ModelState.AddModelError(nameof(model.CategoryId), "Geçersiz kategori.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(model);
            return View(model);
        }

        var subscription = new Subscription
        {
            Name = model.Name.Trim(),
            CategoryId = model.CategoryId,
            Price = model.Price,
            BillingPeriod = model.BillingPeriod,
            StartDate = model.StartDate,
            NextPaymentDate = model.NextPaymentDate,
            IsActive = model.IsActive,
            Description = model.Description?.Trim(),
            UserId = CurrentUserId
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Abonelik başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesAsync(SubscriptionFormViewModel model)
    {
        model.Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
            .ToListAsync();
    }
}