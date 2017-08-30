using System.Web.Http.Filters;

namespace HangMan.Api.Controllers.Filters
{
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
        }
    }
}