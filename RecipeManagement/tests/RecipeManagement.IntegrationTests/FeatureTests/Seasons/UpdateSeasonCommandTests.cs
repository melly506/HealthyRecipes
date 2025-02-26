namespace RecipeManagement.IntegrationTests.FeatureTests.Seasons;

using RecipeManagement.SharedTestHelpers.Fakes.Season;
using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Domain.Seasons.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateSeasonCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_season_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var season = new FakeSeasonBuilder().Build();
        await testingServiceScope.InsertAsync(season);
        var updatedSeasonDto = new FakeSeasonForUpdateDto().Generate();

        // Act
        var command = new UpdateSeason.Command(season.Id, updatedSeasonDto);
        await testingServiceScope.SendAsync(command);
        var updatedSeason = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Seasons
                .FirstOrDefaultAsync(s => s.Id == season.Id));

        // Assert
        updatedSeason.Name.Should().Be(updatedSeasonDto.Name);
    }
}