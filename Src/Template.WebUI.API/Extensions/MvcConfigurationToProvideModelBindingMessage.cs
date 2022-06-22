using ATA.Food.Shared.Localization.Resources.ModelBindingErrorMessages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Template.WebUI.API.Extensions
{
    /// <summary>
    /// To hook into the MVC configuration, we implement the IConfigureOptions_MvcOptions> interface.
    /// </summary>
    public class MvcConfigurationToProvideModelBindingMessage : IConfigureOptions<MvcOptions>
    {
        private readonly IStringLocalizer<ModelBindingStrings> _stringLocalizer;
        public MvcConfigurationToProvideModelBindingMessage(IStringLocalizer<ModelBindingStrings> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        public void Configure(MvcOptions options)
        {
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => _stringLocalizer.GetString(nameof(ModelBindingStrings.ValueIsInvalidAccessor), x));
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => _stringLocalizer.GetString(nameof(ModelBindingStrings.ValueMustBeANumberAccessor), x));
            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => _stringLocalizer.GetString(nameof(ModelBindingStrings.MissingBindRequiredValueAccessor), x));
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => _stringLocalizer.GetString(nameof(ModelBindingStrings.AttemptedValueIsInvalidAccessor), x, y));
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => _stringLocalizer.GetString(nameof(ModelBindingStrings.MissingKeyOrValueAccessor)));
            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => _stringLocalizer.GetString(nameof(ModelBindingStrings.UnknownValueIsInvalidAccessor), x));
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => _stringLocalizer.GetString(nameof(ModelBindingStrings.ValueMustNotBeNullAccessor), x));
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => _stringLocalizer.GetString(nameof(ModelBindingStrings.NonPropertyAttemptedValueIsInvalidAccessor), x));
            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => _stringLocalizer.GetString(nameof(ModelBindingStrings.UnknownValueIsInvalidAccessor)));
            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => _stringLocalizer.GetString(nameof(ModelBindingStrings.NonPropertyValueMustBeANumberAccessor)));
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => _stringLocalizer.GetString(nameof(ModelBindingStrings.MissingRequestBodyRequiredValueAccessor)));
        }
    }
}