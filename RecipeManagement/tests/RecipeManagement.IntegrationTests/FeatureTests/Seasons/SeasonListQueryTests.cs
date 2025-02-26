namespace RecipeManagement.IntegrationTests.FeatureTests.Seasons;

using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Season;
using RecipeManagement.Domain.Seasons.Features;
using Domain;
using System.Threading.Tasks;

public class SeasonListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_season_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var seasonOne = new FakeSeasonBuilder().Build();
        var seasonTwo = new FakeSeasonBuilder().Build();
        var queryParameters = new SeasonParametersDto();

        await testingServiceScope.InsertAsync(seasonOne, seasonTwo);

        // Act
        var query = new GetSeasonList.Query(queryParameters);
        var seasons = await testingServiceScope.SendAsync(query);

        // Assert
        seasons.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}