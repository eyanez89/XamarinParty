using HangMan.API.Middleware.Authentication;
using HangMan.Data.EFContext;
using HangMan.Service;
using Microsoft.Owin;
using Owin;
using Swashbuckle.Application;
using System.Linq;
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

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "HangMan API")
                 .Description("API para el desarrollo de HangMan en Xamarin Party");
                c.ResolveConflictingActions(x => x.First());
            }).EnableSwaggerUi();

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