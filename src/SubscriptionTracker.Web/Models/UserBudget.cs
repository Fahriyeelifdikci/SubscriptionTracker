using Microsoft.AspNetCore.Identity;

namespace SubscriptionTracker.Web.Models;

public class UserBudget
{
    public int Id { get; set; }
    public decimal MonthlyLimit { get; set; }

    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;
}
//Her kullanıcının tek bir bütçe kaydı olacak. Bu "bir kullanıcıya bir kayıt" kuralını sonraki adımda DbContext'te benzersiz indeks ile veritabanı seviyesinde garanti edeceğiz.