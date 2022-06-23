using Bit.Core.Contracts;
using Bit.Core.Exceptions;
using Bit.Owin.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using System.Globalization;
using Template.Application.DTOs;
using Template.Domain.Entities.Identity;
using Template.Domain.Shared;
using ILogger = Bit.Core.Contracts.ILogger;

namespace Template.WebUI.API.Middlewares;

public class IdentityTokenMiddleware
{
    private readonly RequestDelegate _next;

    private readonly string[] _identityTokenValidSubUris =
    {
        "/api",
        "/odata"
    };

    public IdentityTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (_identityTokenValidSubUris.Any(m =>
                    context.Request.Path.HasValue &&
                    context.Request.Path.Value.StartsWith(m, StringComparison.OrdinalIgnoreCase)
                ))
            {

                var userInformationProvider = context.RequestServices.GetRequiredService<IUserInformationProvider>();

                if (userInformationProvider.IsAuthenticated())
                {
                    var identityTokenId = Convert.ToInt32(
                        userInformationProvider.GetBitJwtToken()
                            .Claims
                            .ExtendedSingle("Finding identity token id", p => p.Key == AppConstants.IdentityTokenId)
                            .Value!
                    );

                    var mediator = context.RequestServices.GetRequiredService<IMediator>();

                    IdentityTokenEntity identityToken =
                        await mediator.Send(new GetIdentityTokenByIdQuery(identityTokenId));

                    if (identityToken == null)
                        throw new UnauthorizedException();

                    var dateTimeProvider = context.RequestServices.GetRequiredService<IDateTimeProvider>();

                    if (dateTimeProvider.GetCurrentUtcDateTime() > identityToken.ExpiresAt)
                        throw new UnauthorizedException();
                }
            }

            await _next.Invoke(context);
        }
        catch (Exception exp) // workaround
        {
            var scopeStatusManager = context.RequestServices.GetRequiredService<IScopeStatusManager>();
            var logger = context.RequestServices.GetRequiredService<ILogger>();
            if (scopeStatusManager.WasSucceeded())
                scopeStatusManager.MarkAsFailed(exp.Message);
            logger.AddLogData("Request-Execution-Exception", exp);
            string statusCode = context.Response.StatusCode.ToString(CultureInfo.InvariantCulture);
            bool responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason = statusCode.StartsWith("5", StringComparison.OrdinalIgnoreCase);
            bool responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason = statusCode.StartsWith("4", StringComparison.OrdinalIgnoreCase);
            if (responseStatusCodeIsErrorCodeBecauseOfSomeClientBasedReason == false && responseStatusCodeIsErrorCodeBecauseOfSomeServerBasedReason == false)
            {
                IExceptionToHttpErrorMapper exceptionToHttpErrorMapper = context.RequestServices.GetRequiredService<IExceptionToHttpErrorMapper>();
                context.Response.StatusCode = Convert.ToInt32(exceptionToHttpErrorMapper.GetStatusCode(exp), CultureInfo.InvariantCulture);
                context.Features.Get<IHttpResponseFeature>().ReasonPhrase = exceptionToHttpErrorMapper.GetReasonPhrase(exp);
                await context.Response.WriteAsync(exceptionToHttpErrorMapper.GetMessage(exp), context.RequestAborted);
            }
            return;
        }

        await _next.Invoke(context);
    }
}