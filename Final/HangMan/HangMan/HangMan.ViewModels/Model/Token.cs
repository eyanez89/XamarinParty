using HangMan.DependencyInterface;
using HangMan.Model.Model;
using HangMan.Service.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangMan.ViewModels
{
    public class Token
    {
        private IInternalStorage internalStorage;
        public Token() : this(DependencyService.Get<IInternalStorage>())
        {
        }

        public Token(IInternalStorage internalStorage)
        {
            this.internalStorage = internalStorage;
        }

        public void StoreToken(OAuthRefreshTokenSchema auth)
        {
            internalStorage.PutString("access_token", auth.Access_token);
            internalStorage.PutString("refresh_token", auth.Refresh_token);
            internalStorage.PutString("token_type", auth.Token_type);
            internalStorage.PutString("expires_in", DateTime.Now.AddSeconds(auth.Expires_in * 0.75).ToString());
        }

        public async Task<string> GetToken()
        {
            var expirationDate = Convert.ToDateTime(internalStorage.GetString("expires_in"));
            var now = DateTime.Now;

            var auth = new OAuthRefreshTokenSchema()
            {
                Access_token = internalStorage.GetString("access_token"),
                Refresh_token = internalStorage.GetString("refresh_token"),
                Token_type = internalStorage.GetString("token_type"),
                Expires_in = ((expirationDate.Second) - (now.Second)),
            };

            if (auth.Expires_in > 0)
                return auth.Access_token;

            var authService = new AuthService();
            auth = await authService.Token(auth.Refresh_token);
            StoreToken(auth);

            return auth.Access_token;
        }
    }
}
