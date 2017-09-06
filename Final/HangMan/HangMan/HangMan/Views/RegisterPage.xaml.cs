using HangMan.DependencyInterface;
using HangMan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HangMan.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private RegisterViewModel ViewModel;
        private INotificationService notificationService;

        public RegisterPage()
        {
            InitializeComponent();

            ViewModel = new RegisterViewModel();
            BindingContext = ViewModel;

            notificationService = DependencyService.Get<INotificationService>();
        }

        protected override void OnAppearing()
        {
            SubscribeToMessages();

            base.OnAppearing();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<RegisterViewModel>(this, "Authorized", (sender) =>
            {
                App.Current.MainPage = new MainPage();
            });
            MessagingCenter.Subscribe<RegisterViewModel>(this, "CannotCreateUser", (sender) =>
            {
                Navigation.PopAsync();
            });
            MessagingCenter.Subscribe<RegisterViewModel>(this, "Unauthorized", (sender) =>
            {
                notificationService.Notify("Usuario o contraseña incorrectos.");
            });
            MessagingCenter.Subscribe<RegisterViewModel>(this, "Disconected", (sender) =>
            {
                notificationService.Notify("Verifique su conexión a internet y vuelva a intentarlo.");
            });
            MessagingCenter.Subscribe<RegisterViewModel>(this, "Exception", (sender) =>
            {
                notificationService.Notify("No se puede completar la acción en este momento, vuelva a intentarlo mas tarde.");
            });
        }

        protected override void OnDisappearing()
        {
            UnsubscribeFromMessages();

            base.OnDisappearing();
        }

        private void UnsubscribeFromMessages()
        {
            MessagingCenter.Unsubscribe<RegisterViewModel>(this, "Authorized");
            MessagingCenter.Unsubscribe<RegisterViewModel>(this, "CannotCreateUser");
            MessagingCenter.Unsubscribe<RegisterViewModel>(this, "Disconected");
            MessagingCenter.Unsubscribe<RegisterViewModel>(this, "Unauthorized");
            MessagingCenter.Unsubscribe<RegisterViewModel>(this, "Exception");
        }
    }
}