public partial class App : Application
{
    public App() => InitializeComponent();

    protected override async void OnStart()
    {
        // Check if JWT exists; if not, show login
        if (await SecureStorage.GetAsync("jwt") == null)
            MainPage = new NavigationPage(new Views.LoginPage());
        else
            MainPage = new NavigationPage(new Views.CustomersPage());
    }
}
