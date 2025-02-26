namespace RecipeManagement.UnitTests.Domain.UserFavorites;

using RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateUserFavoriteTests
{
    private readonly Faker _faker;

    public UpdateUserFavoriteTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_userFavorite()
    {
        // Arrange
        var userFavorite = new FakeUserFavoriteBuilder().Build();
        var updatedUserFavorite = new FakeUserFavoriteForUpdate().Generate();
        
        // Act
        userFavorite.Update(updatedUserFavorite);

        // Assert
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var userFavorite = new FakeUserFavoriteBuilder().Build();
        var updatedUserFavorite = new FakeUserFavoriteForUpdate().Generate();
        userFavorite.DomainEvents.Clear();
        
        // Act
        userFavorite.Update(updatedUserFavorite);

        // Assert
        userFavorite.DomainEvents.Count.Should().Be(1);
        userFavorite.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserFavoriteUpdated));
    }
}