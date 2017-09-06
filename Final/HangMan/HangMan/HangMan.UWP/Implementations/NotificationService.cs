using System;
using HangMan.DependencyInterface;
using HangMan.UWP.Implementations;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationService))]
namespace HangMan.UWP.Implementations
{
    public class NotificationService : INotificationService
    {
        public void Notify(string message)
        {
            MessageBox.Show(message);
        }
    }
}