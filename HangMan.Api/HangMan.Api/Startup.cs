using HangMan.API.Middleware.Authentication;
using HangMan.Data.EFContext;
using HangMan.Service;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(HangMan.Api.Startup))]
namespace HangMan.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseJWTAuthentication(new UserService(new HangManContext()));
        }
    }
}