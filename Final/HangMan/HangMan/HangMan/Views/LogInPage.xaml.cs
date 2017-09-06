using HangMan.DependencyInterface;
using HangMan.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HangMan.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        private LogInViewModel ViewModel { get; set; }
        private INotificationService notificationService;

        public LogInPage()
        {
            InitializeComponent();

            ViewModel = new LogInViewModel();
            BindingContext = ViewModel;

            notificationService = DependencyService.Get<INotificationService>();
        }

        private async void OnRegister(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        protected override void OnAppearing()
        {
            SubscribeToMessages();

            base.OnAppearing();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<LogInViewModel>(this, "Authorized", (sender) =>
            {
                App.Current.MainPage = new MainPage();
            });
            MessagingCenter.Subscribe<LogInViewModel>(this, "Unauthorized", (sender) =>
            {
                notificationService.Notify("Usuario o contraseña incorrectos.");
            });
            MessagingCenter.Subscribe<LogInViewModel>(this, "Disconected", (sender) =>
            {
                notificationService.Notify("Verifique su conexión a internet y vuelva a intentarlo.");
            });
            MessagingCenter.Subscribe<LogInViewModel>(this, "Exception", (sender) =>
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
            MessagingCenter.Unsubscribe<LogInViewModel>(this, "Authorized");
            MessagingCenter.Unsubscribe<LogInViewModel>(this, "Disconected");
            MessagingCenter.Unsubscribe<LogInViewModel>(this, "Unauthorized");
            MessagingCenter.Unsubscribe<LogInViewModel>(this, "Exception");
        }
    }
}