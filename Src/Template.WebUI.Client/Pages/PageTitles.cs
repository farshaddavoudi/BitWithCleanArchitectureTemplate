using Template.Domain.Shared;

namespace Template.WebUI.Client.Pages;

public static class PageTitles
{
    public static class HomePage
    {
        public static readonly string Title = AppMetadata.PersianFullName;
    }

    public static class LogsPage
    {
        public static readonly string Title = "Audit Logs";
    }

    public static class PersonnelReportPage
    {
        public static readonly string ManagerialReportTitle = "گزارش غذای پرسنل";
        public static readonly string PersonnelMyFoodTitle = "گزارش غذای من";
    }

    public static class GuestReportPage
    {
        public static readonly string Title = "گزارش غذای مهمان";
    }

    public static class StatisticalReportPage
    {
        public static readonly string Title = "گزارش آماری";
    }

    public static class UsersPage
    {
        public static readonly string Title = "کاربران";
    }

    public static class RolesPage
    {
        public static readonly string Title = "نقش‌ها";
    }

    public static class SettingsPage
    {
        public static readonly string Title = "تنظیمات";
    }

    public static class FoodMenuPage
    {
        public static readonly string Title = "تعریف غذا";
    }

    public static class FoodCalendarPage
    {
        public static readonly string Title = "تعریف گروه و تقویم غذایی";
    }

    public static class SelectFoodPage
    {
        public static readonly string Title = "انتخاب غذا آتا";
    }
}