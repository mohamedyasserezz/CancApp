namespace CanaApp.Domain.Contract.Service.Notification
{
    public interface INotificationServices
    {
        Task SendNotificationAsync(string fcmToken, string title, string body);
    }
}
