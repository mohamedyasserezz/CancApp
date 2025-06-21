using CanaApp.Domain.Contract.Service.Notification;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace CanaApp.Persistance.Services
{
    public class FirebaseService : INotificationServices
    {
        public FirebaseService()
        {
            // Initialize Firebase Admin SDK
            var serviceAccountPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "canc-app-firebase-adminsdk-i67fr-b8ee1e02be.json");

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(serviceAccountPath)
                });
            }
        }
        public async Task SendNotificationAsync(string fcmToken, string title, string body)
        {
            var message = new Message()
            {
                Token = fcmToken,
                Data = new Dictionary<string, string>
            {
                { "title", title },
                { "body", body }
            }
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
