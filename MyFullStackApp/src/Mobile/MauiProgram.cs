public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(f => f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services.AddSingleton<Services.ApiService>();
        builder.Services.AddSingleton<Services.PushNotificationService>();

        return builder.Build();
    }
}
