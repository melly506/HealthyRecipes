namespace RecipeManagement.Domain.Comments.Models;

using Destructurama.Attributed;

public sealed record CommentForCreation
{
    public string Text { get; set; }

}
