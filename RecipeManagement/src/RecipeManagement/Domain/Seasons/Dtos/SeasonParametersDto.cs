namespace RecipeManagement.Domain.Seasons.Dtos;

using RecipeManagement.Resources;

public sealed class SeasonParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
