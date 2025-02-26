namespace RecipeManagement.UnitTests.Domain.Comments;

using RecipeManagement.SharedTestHelpers.Fakes.Comment;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateCommentTests
{
    private readonly Faker _faker;

    public UpdateCommentTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_comment()
    {
        // Arrange
        var comment = new FakeCommentBuilder().Build();
        var updatedComment = new FakeCommentForUpdate().Generate();
        
        // Act
        comment.Update(updatedComment);

        // Assert
        comment.Text.Should().Be(updatedComment.Text);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var comment = new FakeCommentBuilder().Build();
        var updatedComment = new FakeCommentForUpdate().Generate();
        comment.DomainEvents.Clear();
        
        // Act
        comment.Update(updatedComment);

        // Assert
        comment.DomainEvents.Count.Should().Be(1);
        comment.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(CommentUpdated));
    }
}