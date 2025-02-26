namespace RecipeManagement.SharedTestHelpers.Fakes.Comment;

using AutoBogus;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Models;

public sealed class FakeCommentForCreation : AutoFaker<CommentForCreation>
{
    public FakeCommentForCreation()
    {
    }
}