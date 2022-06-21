using ATABit.Server.Extensions;
using ATABit.Shared;
using ATABit.Shared.Exceptions;
using Bit.Core.Contracts;
using Microsoft.Owin;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ILogger = Bit.Core.Contracts.ILogger;

namespace Template.WebUI.API.ActionFilters
{
    public class ModelStateValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState is null)
                throw new ArgumentNullException(nameof(actionContext.ModelState));

            if (actionContext.ModelState.IsValid | actionContext.Request.Method == HttpMethod.Get /*workaround*/)
                return;

            IDependencyResolver dependencyResolver = actionContext.Request.GetOwinContext().GetDependencyResolver();
            var logger = dependencyResolver.Resolve<ILogger>();

            ModelErrorWrapper modelErrorWrapper = actionContext.ModelState.GetModelErrors();
            logger.AddLogData("ModelError", modelErrorWrapper);

            throw new ValidationException(modelErrorWrapper);
        }
    }
}