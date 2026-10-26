using Microsoft.AspNetCore.Mvc.Rendering;
using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.ViewModels;

public class SubscriptionIndexViewModel
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsActive { get; set; }
    public BillingPeriod? BillingPeriod { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    public List<SubscriptionListItemViewModel> Items { get; set; } = new();

    public bool HasFilters =>
        !string.IsNullOrWhiteSpace(Search) || CategoryId.HasValue || IsActive.HasValue || BillingPeriod.HasValue;
}