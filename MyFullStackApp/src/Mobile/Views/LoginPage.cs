public partial class LoginPage : ContentPage
{
    private readonly ApiService _api = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    async void OnLoginClicked(object s, EventArgs e)
    {
        if (await _api.LoginAsync(userEntry.Text, passEntry.Text))
            await Navigation.PushAsync(new CustomersPage());
        else
            await DisplayAlert("Error", "Invalid credentials", "OK");
    }
}
