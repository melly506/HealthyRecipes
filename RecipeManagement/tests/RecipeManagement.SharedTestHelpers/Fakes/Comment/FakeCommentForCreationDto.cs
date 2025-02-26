namespace RecipeManagement.SharedTestHelpers.Fakes.Comment;

using AutoBogus;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Dtos;

public sealed class FakeCommentForCreationDto : AutoFaker<CommentForCreationDto>
{
    public FakeCommentForCreationDto()
    {
    }
}