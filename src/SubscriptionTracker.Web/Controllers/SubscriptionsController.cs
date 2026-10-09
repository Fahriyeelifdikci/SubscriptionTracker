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

    public async Task<IActionResult> Details(int id)
    {
        var userId = CurrentUserId;

        var model = await _context.Subscriptions
            .Where(s => s.Id == id && s.UserId == userId)
            .Select(s => new SubscriptionDetailsViewModel
            {
                Id = s.Id,
                Name = s.Name,
                CategoryName = s.Category.Name,
                Price = s.Price,
                BillingPeriod = s.BillingPeriod,
                StartDate = s.StartDate,
                NextPaymentDate = s.NextPaymentDate,
                IsActive = s.IsActive,
                Description = s.Description
            })
            .FirstOrDefaultAsync();

        if (model is null)
            return NotFound();

        return View(model);
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
        await ValidateCategoryAsync(model);

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(model);
            return View(model);
        }

        var subscription = new Subscription { UserId = CurrentUserId };
        ApplyForm(subscription, model);

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Abonelik başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var subscription = await FindOwnedAsync(id);
        if (subscription is null)
            return NotFound();

        var model = new SubscriptionFormViewModel
        {
            Name = subscription.Name,
            CategoryId = subscription.CategoryId,
            Price = subscription.Price,
            BillingPeriod = subscription.BillingPeriod,
            StartDate = subscription.StartDate,
            NextPaymentDate = subscription.NextPaymentDate,
            IsActive = subscription.IsActive,
            Description = subscription.Description
        };

        await PopulateCategoriesAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubscriptionFormViewModel model)
    {
        var subscription = await FindOwnedAsync(id);
        if (subscription is null)
            return NotFound();

        await ValidateCategoryAsync(model);

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(model);
            return View(model);
        }

        ApplyForm(subscription, model);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Abonelik güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<Subscription?> FindOwnedAsync(int id)
    {
        var userId = CurrentUserId;

        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
    }

    private async Task ValidateCategoryAsync(SubscriptionFormViewModel model)
    {
        if (ModelState.IsValid && !await _context.Categories.AnyAsync(c => c.Id == model.CategoryId))
        {
            ModelState.AddModelError(nameof(model.CategoryId), "Geçersiz kategori.");
        }
    }

    private static void ApplyForm(Subscription subscription, SubscriptionFormViewModel model)
    {
        subscription.Name = model.Name.Trim();
        subscription.CategoryId = model.CategoryId;
        subscription.Price = model.Price;
        subscription.BillingPeriod = model.BillingPeriod;
        subscription.StartDate = model.StartDate;
        subscription.NextPaymentDate = model.NextPaymentDate;
        subscription.IsActive = model.IsActive;
        subscription.Description = model.Description?.Trim();
    }

    private async Task PopulateCategoriesAsync(SubscriptionFormViewModel model)
    {
        model.Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
            .ToListAsync();
    }
}