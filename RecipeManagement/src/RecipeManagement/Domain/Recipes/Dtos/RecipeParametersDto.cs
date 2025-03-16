namespace RecipeManagement.Domain.Recipes.Dtos;

using RecipeManagement.Resources;

public sealed class RecipeParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
    public string? FoodTypeId { get; set; }
}
