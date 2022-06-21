namespace Template.Domain.Shared
{
    public class AppMessages
    {
        public class Errors
        {
            public static string FoodIsDuplicate = "این غذا قبلا در سیستم ثبت شده است";
            public static string FoodDeletionFailed = "مشکلی در حذف غذا وجود دارد";
            public static string FoodUsedBefore = "از این غذا قبلا استفاده شده و امکان حذف آن وجود ندارد";
            public static string FoodIdNotFound = "غذایی با این شناسه پیدا نشد";
        }

        public class Success
        {
            public static string FoodAddedSuccessfully = "غذا با موفقیت ثبت شد";
            public static string FoodDeletedSuccessfully = "غذا با موفقیت حذف شد";
        }
    }
}
