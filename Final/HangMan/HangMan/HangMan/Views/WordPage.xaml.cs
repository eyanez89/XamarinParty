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
    public partial class WordPage : ContentPage
    {
        private WordViewModel ViewModel;
        private INotificationService notificationService;

        public WordPage()
        {
            InitializeComponent();

            ViewModel = new WordViewModel();
            BindingContext = ViewModel;

            notificationService = DependencyService.Get<INotificationService>();
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<LogInViewModel>(this, "Added", async (sender) =>
            {
                await Navigation.PopAsync();
            });
            MessagingCenter.Subscribe<LogInViewModel>(this, "Exception", (sender) =>
            {
                notificationService.Notify("No se puede completar la acción en este momento, vuelva a intentarlo mas tarde.");
            });

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<LogInViewModel>(this, "Added");
            MessagingCenter.Unsubscribe<LogInViewModel>(this, "Exception");

            base.OnDisappearing();
        }
    }
}