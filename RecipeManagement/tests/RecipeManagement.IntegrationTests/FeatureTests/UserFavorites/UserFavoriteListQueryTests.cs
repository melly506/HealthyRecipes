namespace RecipeManagement.IntegrationTests.FeatureTests.UserFavorites;

using RecipeManagement.Domain.UserFavorites.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;
using RecipeManagement.Domain.UserFavorites.Features;
using Domain;
using System.Threading.Tasks;

public class UserFavoriteListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_userfavorite_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var userFavoriteOne = new FakeUserFavoriteBuilder().Build();
        var userFavoriteTwo = new FakeUserFavoriteBuilder().Build();
        var queryParameters = new UserFavoriteParametersDto();

        await testingServiceScope.InsertAsync(userFavoriteOne, userFavoriteTwo);

        // Act
        var query = new GetUserFavoriteList.Query(queryParameters);
        var userFavorites = await testingServiceScope.SendAsync(query);

        // Assert
        userFavorites.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}