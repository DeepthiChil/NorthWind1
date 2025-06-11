public partial class CustomersPage : ContentPage
{
    private readonly ApiService _api = new();
    private readonly PushNotificationService _push = new();

    public CustomersPage()
    {
        InitializeComponent();
        LoadCustomers();
        _push.Init();
    }

    async void LoadCustomers()
    {
        var list = await _api.GetCustomersAsync();
        cv.ItemsSource = list;
    }

    async void OnCustomerSelected(object s, SelectionChangedEventArgs e)
    {
        var cust = e.CurrentSelection.FirstOrDefault() as Models.Customer;
        if (cust != null)
            await Navigation.PushAsync(new OrdersPage(cust));
    }
}
