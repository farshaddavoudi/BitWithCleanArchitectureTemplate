using System.Text;
using System.Web.Http.Filters;
using Bit.Owin.Contracts;
using Bit.WebApi.ActionFilters;

namespace Template.WebUI.API.ActionFilters;

public class ATAExceptionHandlerFilterAttribute : ExceptionHandlerFilterAttribute
{
    protected override HttpResponseMessage CreateErrorResponseMessage(HttpActionExecutedContext actionExecutedContext, IExceptionToHttpErrorMapper exceptionToHttpErrorMapper, Exception exception)
    {
        return new HttpResponseMessage
        {
            Content = new StringContent(exceptionToHttpErrorMapper.GetMessage(actionExecutedContext.Exception), Encoding.UTF8, "application/json"),
            StatusCode = exceptionToHttpErrorMapper.GetStatusCode(exception)
        };
    }
}