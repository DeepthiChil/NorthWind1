using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace MyFullStackApp.Backend;

public class FirebasePushService : IFirebasePushService
{
    public FirebasePushService(IConfiguration config)
    {
        var path = config["Firebase:ServiceAccountKeyPath"];
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(path)
        });
    }

    public async Task SendNotificationAsync(string type)
    {
        var message = new Message()
        {
            Topic = "data-changed",
            Notification = new Notification
            {
                Title = "Data Update",
                Body = $"New {type} data available."
            }
        };
        await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
}
