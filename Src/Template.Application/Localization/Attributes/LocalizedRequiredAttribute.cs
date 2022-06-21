using System.ComponentModel.DataAnnotations;
using ATA.Food.Shared.Localization.Resources.DataAnnotations;

namespace Template.Application.Localization.Attributes
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        public LocalizedRequiredAttribute()
        {
            ErrorMessageResourceType = typeof(DataAnnotationStrings);
            ErrorMessageResourceName = nameof(DataAnnotationStrings.RequiredAttribute_ValidationError);
        }

        public LocalizedRequiredAttribute(string dataAnnotationStringsResourceKey)
        {
            ErrorMessageResourceType = typeof(DataAnnotationStrings);
            ErrorMessageResourceName = dataAnnotationStringsResourceKey;
        }
    }
}