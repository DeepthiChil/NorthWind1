public partial class OrdersPage : ContentPage
{
    private readonly ApiService _api = new();
    private readonly Models.Customer _customer;

    public OrdersPage(Models.Customer cust)
    {
        InitializeComponent();
        _customer = cust;
        Title = _customer.CompanyName;
        LoadOrders();
    }

    async void LoadOrders()
    {
        var list = await _api.GetOrdersAsync(_customer.CustomerID);
        cvOrders.ItemsSource = list;
    }

    async void OnNewOrder(object s, EventArgs e)
    {
        var order = new Models.Order {
            CustomerID = _customer.CustomerID,
            OrderDate = DateTime.UtcNow,
            ShipCity = "Default City"
        };
        if (await _api.AddOrderAsync(order))
            LoadOrders();
    }
}
