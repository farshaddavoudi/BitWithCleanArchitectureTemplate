using System.Reflection;

namespace Template.Domain.Shared;

public static class AppMetadata
{
    public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString(3);

    public static readonly string SSOAppName = "food";

    public static readonly string PersianFullName = "سامانه‌ی انتخاب غذای آتا";

    public static readonly string EnglishFullName = "ATA Food";

    public static readonly string SolutionName = "ATA.Food";
}