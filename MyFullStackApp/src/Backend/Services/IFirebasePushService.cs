using System.Threading.Tasks;

namespace MyFullStackApp.Backend;

public interface IFirebasePushService
{
    Task SendNotificationAsync(string type);
}
