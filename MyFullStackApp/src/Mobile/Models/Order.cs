namespace MyFullStackApp.Mobile.Models;
public class Order
{
    public int OrderID { get; set; }
    public string CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShipCity { get; set; }
}
