namespace RecipeManagement.UnitTests.Domain.Seasons;

using RecipeManagement.SharedTestHelpers.Fakes.Season;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateSeasonTests
{
    private readonly Faker _faker;

    public UpdateSeasonTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_season()
    {
        // Arrange
        var season = new FakeSeasonBuilder().Build();
        var updatedSeason = new FakeSeasonForUpdate().Generate();
        
        // Act
        season.Update(updatedSeason);

        // Assert
        season.Name.Should().Be(updatedSeason.Name);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var season = new FakeSeasonBuilder().Build();
        var updatedSeason = new FakeSeasonForUpdate().Generate();
        season.DomainEvents.Clear();
        
        // Act
        season.Update(updatedSeason);

        // Assert
        season.DomainEvents.Count.Should().Be(1);
        season.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(SeasonUpdated));
    }
}