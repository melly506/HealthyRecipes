namespace RecipeManagement.IntegrationTests.FeatureTests.Seasons;

using RecipeManagement.SharedTestHelpers.Fakes.Season;
using RecipeManagement.Domain.Seasons.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteSeasonCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_season_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var season = new FakeSeasonBuilder().Build();
        await testingServiceScope.InsertAsync(season);

        // Act
        var command = new DeleteSeason.Command(season.Id);
        await testingServiceScope.SendAsync(command);
        var seasonResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Seasons
                .CountAsync(s => s.Id == season.Id));

        // Assert
        seasonResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_season_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteSeason.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_season_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var season = new FakeSeasonBuilder().Build();
        await testingServiceScope.InsertAsync(season);

        // Act
        var command = new DeleteSeason.Command(season.Id);
        await testingServiceScope.SendAsync(command);
        var deletedSeason = await testingServiceScope.ExecuteDbContextAsync(db => db.Seasons
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == season.Id));

        // Assert
        deletedSeason?.IsDeleted.Should().BeTrue();
    }
}