namespace MyFullStackApp.Backend.Models;

public class Customer
{
    public string CustomerID { get; set; } = Guid.NewGuid().ToString();
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string City { get; set; }

    public ICollection<Order> Orders { get; set; }
}
