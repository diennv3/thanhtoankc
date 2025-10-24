using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

public static class FirebaseHelper
{
    private static bool _isInitialized = false;
    private static readonly object _lock = new object();

    public static void InitializeFirebase(string credentialPath)
    {
        if (!_isInitialized)
        {
            lock (_lock)
            {
                if (!_isInitialized)
                {
                    if (string.IsNullOrEmpty(credentialPath))
                        throw new ArgumentException("Firebase credential path is required!");

                    if (FirebaseApp.DefaultInstance == null)
                    {
                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromFile(credentialPath)
                        });
                    }

                    _isInitialized = true;
                }
            }
        }
    }

    public static async Task<string> SendFCM(string deviceToken, string title, string body)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("FirebaseApp is not initialized! Call InitializeFirebase() once at startup.");

        var message = new Message()
        {
            Token = deviceToken,
            Notification = new Notification
            {
                Title = title,
                Body = body
            }
        };

        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        return response;
    }
}