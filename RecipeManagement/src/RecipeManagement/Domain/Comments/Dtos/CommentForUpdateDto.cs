namespace RecipeManagement.Domain.Comments.Dtos;

using Destructurama.Attributed;

public sealed record CommentForUpdateDto
{
    public string Text { get; set; }

}
