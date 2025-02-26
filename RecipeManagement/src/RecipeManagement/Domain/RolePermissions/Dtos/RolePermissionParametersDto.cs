namespace RecipeManagement.Domain.RolePermissions.Dtos;

using RecipeManagement.Resources;

public sealed class RolePermissionParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
