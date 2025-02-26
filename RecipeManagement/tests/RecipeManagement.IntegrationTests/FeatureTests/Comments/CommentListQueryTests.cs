namespace RecipeManagement.IntegrationTests.FeatureTests.Comments;

using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Comment;
using RecipeManagement.Domain.Comments.Features;
using Domain;
using System.Threading.Tasks;

public class CommentListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_comment_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var commentOne = new FakeCommentBuilder().Build();
        var commentTwo = new FakeCommentBuilder().Build();
        var queryParameters = new CommentParametersDto();

        await testingServiceScope.InsertAsync(commentOne, commentTwo);

        // Act
        var query = new GetCommentList.Query(queryParameters);
        var comments = await testingServiceScope.SendAsync(query);

        // Assert
        comments.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}