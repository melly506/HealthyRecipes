namespace RecipeManagement.Domain.FoodTypes.Dtos;

using RecipeManagement.Resources;

public sealed class FoodTypeParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
