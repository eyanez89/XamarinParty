using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace HangMan.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest()        {            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")            {                Response.Headers.Add("Access-Control-Allow-Origin", "*");                Response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, X-Auth-Token, Authorization");                Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PATCH, PUT, DELETE, OPTIONS");                Response.Headers.Add("Access-Control-Allow-Credentials", "true");                Response.Headers.Add("Access-Control-Max-Age", "1728000");                Response.End();            }        }
    }
}
