using HangMan.DependencyInterface;
using HangMan.Service.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HangMan.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        public LogInViewModel()
        {
            LogInCommand = new Command(async () => await LogIn());
        }

        private string _userName;
        private string _password;
        private ICommand _logInCommand;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(nameof(UserName)); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public ICommand LogInCommand
        {
            get { return _logInCommand; }
            set { _logInCommand = value; OnPropertyChanged(nameof(LogInCommand)); }
        }

        private async Task LogIn()
        {
            IsBusy = true;

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var authService = new AuthService();
                    var auth = await authService.Token(UserName, Password);
                    var internalStorage = DependencyService.Get<IInternalStorage>();

                    internalStorage.PutString("access_token", auth.Access_token);
                    internalStorage.PutString("refresh_token", auth.Refresh_token);
                    internalStorage.PutString("token_type", auth.Token_type);
                    internalStorage.PutString("expires_in", 
                        DateTime.Now.AddSeconds(auth.Expires_in * 0.75).ToString());

                    MessagingCenter.Send(this, "Authorized");
                }
                else
                    MessagingCenter.Send(this, "Disconected");
            }
            catch (UnauthorizedAccessException)
            {
                MessagingCenter.Send(this, "Unauthorized");
            }
            catch (Exception)
            {
                MessagingCenter.Send(this, "Exception");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
