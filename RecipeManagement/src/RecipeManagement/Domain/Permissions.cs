namespace RecipeManagement.Domain;

using System.Reflection;

public static class Permissions
{
    // Permissions marker - do not delete this comment
    public const string CanDeleteUsers = nameof(CanDeleteUsers);
    public const string CanUpdateUsers = nameof(CanUpdateUsers);
    public const string CanAddUsers = nameof(CanAddUsers);
    public const string CanGetUsers = nameof(CanGetUsers);
    public const string CanDeletePermissions = nameof(CanDeletePermissions);
    public const string CanUpdatePermissions = nameof(CanUpdatePermissions);
    public const string CanAddPermissions = nameof(CanAddPermissions);
    public const string CanReadRolePermissions = nameof(CanReadRolePermissions);
    public const string HangfireAccess = nameof(HangfireAccess);
    public const string CanRemoveUserRoles = nameof(CanRemoveUserRoles);
    public const string CanAddUserRoles = nameof(CanAddUserRoles);
    public const string CanGetRoles = nameof(CanGetRoles);
    public const string CanGetPermissions = nameof(CanGetPermissions);
    
    public static List<string> List()
    {
        return typeof(Permissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(x => (string)x.GetRawConstantValue())
            .ToList();
    }
}
