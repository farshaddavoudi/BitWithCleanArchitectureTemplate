using ATA.Food.Shared.Localization.Resources.ExceptionMessages;
using ATA.Food.Shared.Localization.Resources.GeneralMessages;
using Bit.Core.Exceptions;
using Microsoft.Extensions.Localization;
using Template.Application.Common.Contracts;
using Template.Application.Common.Localization.Resources;

namespace Template.Application.Common.Implementations;

public class StringProvider : IStringProvider
{
    public IStringLocalizer<MessageStrings> MessageStringsLocalizer { get; set; } = default!; //Property Injection
    public IStringLocalizer<ExceptionStrings> ExceptionStringsLocalizer { get; set; } = default!; //Property Injection

    /// <summary>
    /// Get value from MessageStrings resource
    /// </summary>
    /// <param name="messageStringsKey"> Resource key </param>
    /// <returns></returns>
    public string? Message(string messageStringsKey)
    {
        return GetValueByKeyAndResource(messageStringsKey, StringResourceType.MessageStrings);
    }

    /// <summary>
    /// Get value from ExceptionStrings resource
    /// </summary>
    /// <param name="exceptionStringsKey"> Resource key </param>
    /// <returns></returns>
    public string Exception(string exceptionStringsKey)
    {
        return GetValueByKeyAndResource(exceptionStringsKey, StringResourceType.ExceptionStrings) ?? string.Empty;
    }

    private string? GetValueByKeyAndResource(string key, StringResourceType stringResourceType)
    {
        if (stringResourceType == StringResourceType.MessageStrings)
            return MessageStringsLocalizer.GetString(key);

        if (stringResourceType == StringResourceType.ExceptionStrings)
            return ExceptionStringsLocalizer.GetString(key);

        throw new BadRequestException();
    }

}