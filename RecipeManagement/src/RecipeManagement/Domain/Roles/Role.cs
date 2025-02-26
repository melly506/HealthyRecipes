namespace RecipeManagement.Domain.Roles;

using RecipeManagement.Exceptions;
using Ardalis.SmartEnum;

public class Role : ValueObject
{
    private RoleEnum _role;
    public string Value
    {
        get => _role.Name;
        private set
        {
            if (!RoleEnum.TryFromName(value, true, out var parsed))
                throw new ValidationException($"Invalid Role. Please use one of the following: {string.Join(", ", ListNames())}");

            _role = parsed;
        }
    }
    
    public Role(string value)
    {
        Value = value;
    }
    
    public static Role Of(string value) => new Role(value);
    public static implicit operator string(Role value) => value.Value;
    public static List<string> ListNames() => RoleEnum.List.Select(x => x.Name).ToList();

    public static Role User() => new Role(RoleEnum.User.Name);
    public static Role SuperAdmin() => new Role(RoleEnum.SuperAdmin.Name);

    protected Role() { } // EF Core

    private abstract class RoleEnum(string name, int value)
        : SmartEnum<RoleEnum>(name, value)
    {
        public static readonly RoleEnum User = new UserType();
        public static readonly RoleEnum SuperAdmin = new SuperAdminType();

        private class UserType() : RoleEnum("User", 0);
        private class SuperAdminType() : RoleEnum("Super Admin", 1);
    }
}