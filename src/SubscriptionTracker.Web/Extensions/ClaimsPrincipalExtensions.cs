using System.Security.Claims;

namespace SubscriptionTracker.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("Oturumdaki kullanıcı kimliği bulunamadı.");
}