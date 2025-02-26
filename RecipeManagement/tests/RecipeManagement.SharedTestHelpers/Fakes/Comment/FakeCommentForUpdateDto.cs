namespace RecipeManagement.SharedTestHelpers.Fakes.Comment;

using AutoBogus;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Dtos;

public sealed class FakeCommentForUpdateDto : AutoFaker<CommentForUpdateDto>
{
    public FakeCommentForUpdateDto()
    {
    }
}