namespace SubscriptionTracker.Web.Models;

public class Category
{
    public int Id { get; set; }
    //proje nullable reference types açık olduğu için string null olamaz, derleyici varsayılan bir değer ister.
    public string Name { get; set; } = string.Empty; 

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}