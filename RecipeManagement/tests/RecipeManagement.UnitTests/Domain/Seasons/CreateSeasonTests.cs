namespace RecipeManagement.UnitTests.Domain.Seasons;

using RecipeManagement.SharedTestHelpers.Fakes.Season;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateSeasonTests
{
    private readonly Faker _faker;

    public CreateSeasonTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_season()
    {
        // Arrange
        var seasonToCreate = new FakeSeasonForCreation().Generate();
        
        // Act
        var season = Season.Create(seasonToCreate);

        // Assert
        season.Name.Should().Be(seasonToCreate.Name);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var seasonToCreate = new FakeSeasonForCreation().Generate();
        
        // Act
        var season = Season.Create(seasonToCreate);

        // Assert
        season.DomainEvents.Count.Should().Be(1);
        season.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(SeasonCreated));
    }
}