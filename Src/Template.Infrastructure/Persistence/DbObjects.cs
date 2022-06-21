namespace Template.Infrastructure.Persistence;

public static class DbSchemas
{
    public static readonly string ATA = nameof(ATA);
    public static readonly string ATAWorkflowDboSchema = "ATAWorkflow.dbo";
    public static readonly string PardisWebDbDboSchema = "PardisWebDB.dbo";
}

public static class DbViews
{
    public static readonly string UsersView = nameof(UsersView);
    public static readonly string FoodMenu = nameof(FoodMenu);
    public static readonly string UserRolePairsView = nameof(UserRolePairsView);
    public static readonly string LocationView = nameof(LocationView);
}

public static class DbStoredProcedures
{
    // public static readonly string SpName = nameof(SpName);
}