namespace RecipeManagement.IntegrationTests.FeatureTests.Seasons;

using RecipeManagement.SharedTestHelpers.Fakes.Season;
using RecipeManagement.Domain.Seasons.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class SeasonQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_season_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var seasonOne = new FakeSeasonBuilder().Build();
        await testingServiceScope.InsertAsync(seasonOne);

        // Act
        var query = new GetSeason.Query(seasonOne.Id);
        var season = await testingServiceScope.SendAsync(query);

        // Assert
        season.Name.Should().Be(seasonOne.Name);
    }

    [Fact]
    public async Task get_season_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetSeason.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}