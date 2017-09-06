using HangMan.Data;
using HangMan.DependencyInterface;
using HangMan.Model.Model;
using HangMan.Service.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HangMan.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await Register());
        }

        private string _nickName;
        private string _userName;
        private string _password;
        private string _confirmPassword;
        private ICommand _registerCommand;

        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; OnPropertyChanged(nameof(NickName)); }
        }
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
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
        }
        public ICommand RegisterCommand
        {
            get { return _registerCommand; }
            set { _registerCommand = value; OnPropertyChanged(nameof(RegisterCommand)); }
        }

        private async Task Register()
        {
            IsBusy = true;

            try
            {
                if (Password != ConfirmPassword)
                {
                    MessagingCenter.Send(this, "PasswordNotMatch");
                    return;
                }

                if (CrossConnectivity.Current.IsConnected)
                {
                    var user = new User()
                    {
                        UserName = UserName,
                        Password = Password,
                        Player = new Player()
                        {
                            NickName = NickName
                        }
                    };

                    var authService = new AuthService();

                    var serverUser = await authService.Post(user);

                    if (serverUser != null)
                    {
                        var playerData = new PlayerData();
                        await playerData.InsertAsync(serverUser.Player);

                        var auth = await authService.Token(user.UserName, user.Password);
                        var internalStorage = DependencyService.Get<IInternalStorage>();

                        internalStorage.PutString("access_token", auth.Access_token);
                        internalStorage.PutString("refresh_token", auth.Refresh_token);
                        internalStorage.PutString("token_type", auth.Token_type);
                        internalStorage.PutString("expires_in", DateTime.Now.AddSeconds(auth.Expires_in * 0.75).ToString());

                        MessagingCenter.Send(this, "Authorized");
                    }
                    else
                        MessagingCenter.Send(this, "CannotCreateUser");
                }
                else
                    MessagingCenter.Send(this, "Disconected");
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
