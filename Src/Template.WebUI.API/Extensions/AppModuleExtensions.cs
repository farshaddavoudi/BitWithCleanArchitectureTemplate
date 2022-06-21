using Bit.Core.Contracts;
using Template.WebUI.API.ActionFilters;

namespace Template.WebUI.API.Extensions
{
    public static class AppModuleExtensions
    {
        public static IDependencyManager RegisterModelStateValidator(this IDependencyManager dependencyManager)
        {
            dependencyManager.RegisterGlobalWebApiActionFiltersUsing(webApiConfig =>
                webApiConfig.Filters.Add(new ModelStateValidatorAttribute()));

            return dependencyManager;
        }
    }
}