var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseApiUrl"]);
});

// Access token service
builder.Services.AddSingleton<Services.ApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapHub<NotificationHub>("/notifyHub");

app.Run();
