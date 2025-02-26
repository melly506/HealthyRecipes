namespace RecipeManagement.Domain.Ingredients.Dtos;

using RecipeManagement.Resources;

public sealed class IngredientParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
