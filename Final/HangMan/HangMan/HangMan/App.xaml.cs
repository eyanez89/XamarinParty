using HangMan.DependencyInterface;
using Xamarin.Forms;

namespace HangMan.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var internalStorage = DependencyService.Get<IInternalStorage>();
            var refresh_token = internalStorage.GetString("refresh_token");

            if (string.IsNullOrEmpty(refresh_token))
                MainPage = new NavigationPage(new LogInPage());
            else
                MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
