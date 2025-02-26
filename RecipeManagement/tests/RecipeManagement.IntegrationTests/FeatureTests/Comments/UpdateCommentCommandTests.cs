namespace RecipeManagement.IntegrationTests.FeatureTests.Comments;

using RecipeManagement.SharedTestHelpers.Fakes.Comment;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Domain.Comments.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateCommentCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_comment_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var comment = new FakeCommentBuilder().Build();
        await testingServiceScope.InsertAsync(comment);
        var updatedCommentDto = new FakeCommentForUpdateDto().Generate();

        // Act
        var command = new UpdateComment.Command(comment.Id, updatedCommentDto);
        await testingServiceScope.SendAsync(command);
        var updatedComment = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Comments
                .FirstOrDefaultAsync(c => c.Id == comment.Id));

        // Assert
        updatedComment.Text.Should().Be(updatedCommentDto.Text);
    }
}