using Firebase.CloudMessaging;
using Foundation;

namespace MyFullStackApp.Mobile.Services;
public class PushNotificationService : NSObject, IUNUserNotificationCenterDelegate, IMessagingDelegate
{
    public void Init()
    {
        UNUserNotificationCenter.Current.Delegate = this;
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge,
            (approved, err) => Console.WriteLine(approved ? "Notifications Approved" : "Denied"));

        UIApplication.SharedApplication.RegisterForRemoteNotifications();
        Messaging.SharedInstance.Delegate = this;
    }

    [Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
    {
        Console.WriteLine($"FCM Token: {fcmToken}");
        // Optionally send token to backend
    }

    public void DidReceiveMessage(Messaging messaging, Firebase.CloudMessaging.RemoteMessage remoteMessage)
    {
        // Received data message
    }

    [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
    public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        completionHandler(UNNotificationPresentationOptions.Alert);
    }
}
