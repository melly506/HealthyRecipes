namespace RecipeManagement.IntegrationTests.FeatureTests.Seasons;

using RecipeManagement.SharedTestHelpers.Fakes.Season;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Seasons.Features;

public class AddSeasonCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_season_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var seasonOne = new FakeSeasonForCreationDto().Generate();

        // Act
        var command = new AddSeason.Command(seasonOne);
        var seasonReturned = await testingServiceScope.SendAsync(command);
        var seasonCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Seasons
            .FirstOrDefaultAsync(s => s.Id == seasonReturned.Id));

        // Assert
        seasonReturned.Name.Should().Be(seasonOne.Name);

        seasonCreated.Name.Should().Be(seasonOne.Name);
    }
}