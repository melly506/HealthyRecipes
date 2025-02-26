namespace RecipeManagement.Domain.Comments.Models;

using Destructurama.Attributed;

public sealed record CommentForUpdate
{
    public string Text { get; set; }

}
