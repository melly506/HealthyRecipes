namespace RecipeManagement.Domain.UserFavorites.Dtos;

using RecipeManagement.Resources;

public sealed class UserFavoriteParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
