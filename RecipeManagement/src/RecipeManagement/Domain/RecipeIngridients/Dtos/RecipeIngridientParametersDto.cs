namespace RecipeManagement.Domain.RecipeIngridients.Dtos;

using RecipeManagement.Resources;

public sealed class RecipeIngridientParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
