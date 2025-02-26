namespace RecipeManagement.Domain.Comments.Dtos;

using RecipeManagement.Resources;

public sealed class CommentParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
