namespace Template.WebUI.Client.Pages;

public static class PageUrls
{
    public const string SelectFoodPage = "/";

    public static string FlowFormsPage(int paramRequestId) => $"/flow-forms/{paramRequestId}";
    public const string LogsPage = "/logs";

    public const string ReportsRootPath = "/reports";
    public const string MyFoodsReport = $"{ReportsRootPath}/personnel";
    public static string PersonnelReportPage(string city) => $"{ReportsRootPath}/personnel/{city}";
    public static string StatisticalReportPage(string city) => $"{ReportsRootPath}/statistical/{city}";
    public static string GuestsReportPage(string city) => $"{ReportsRootPath}/guests/{city}";

    public const string FoodManagementRootPath = "/foods";
    public const string FoodMenuPage = $"{FoodManagementRootPath}/food-menu";
    public static string FoodCalendarPage(string city) => $"{FoodManagementRootPath}/food-calendar/{city}";

    public const string SettingsRootPath = "/settings";
    public const string UsersPage = $"{SettingsRootPath}/users";
    public const string RolesPage = $"{SettingsRootPath}/roles";
    public const string WorkflowPage = $"{SettingsRootPath}/workflow";
}