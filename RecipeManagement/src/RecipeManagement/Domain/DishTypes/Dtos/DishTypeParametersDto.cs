namespace RecipeManagement.Domain.DishTypes.Dtos;

using RecipeManagement.Resources;

public sealed class DishTypeParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
