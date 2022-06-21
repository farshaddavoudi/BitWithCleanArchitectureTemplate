using ATABit.Server.Extensions;
using ATABit.Shared.Exceptions;
using Bit.Owin.Implementations;
using Bit.Owin.Metadata;
using System.Net;

namespace Template.Application.Common.Exceptions
{
    public class ATAExceptionToHttpErrorMapper : DefaultExceptionToHttpErrorMapper
    {
        public override HttpStatusCode GetStatusCode(Exception exp)
        {
            if (exp is ValidationException)
                return HttpStatusCode.BadRequest;

            //if (exp is InvalidTokenException)
            //    return HttpStatusCode.BadRequest;

            return base.GetStatusCode(exp);
        }

        public override string GetMessage(Exception exp)
        {
            exp = UnWrapException(exp);

            string? messageToShow = BitMetadataBuilder.UnknownError;

            if (IsKnownError(exp))
            {
                messageToShow = exp.GetModelErrorWrapper();
            }

            return messageToShow ?? "";
        }
    }
}