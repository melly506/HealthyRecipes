namespace RecipeManagement.Domain.Users.Dtos;

using RecipeManagement.Resources;

public sealed class UserParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
