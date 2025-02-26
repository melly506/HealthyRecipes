namespace RecipeManagement.SharedTestHelpers.Fakes.Comment;

using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Models;

public class FakeCommentBuilder
{
    private CommentForCreation _creationData = new FakeCommentForCreation().Generate();

    public FakeCommentBuilder WithModel(CommentForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeCommentBuilder WithText(string text)
    {
        _creationData.Text = text;
        return this;
    }
    
    public Comment Build()
    {
        var result = Comment.Create(_creationData);
        return result;
    }
}