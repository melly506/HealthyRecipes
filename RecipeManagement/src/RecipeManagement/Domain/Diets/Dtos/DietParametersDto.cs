namespace RecipeManagement.Domain.Diets.Dtos;

using RecipeManagement.Resources;

public sealed class DietParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
