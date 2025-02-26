namespace RecipeManagement.UnitTests.Domain.UserFavorites;

using RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateUserFavoriteTests
{
    private readonly Faker _faker;

    public CreateUserFavoriteTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_userFavorite()
    {
        // Arrange
        var userFavoriteToCreate = new FakeUserFavoriteForCreation().Generate();
        
        // Act
        var userFavorite = UserFavorite.Create(userFavoriteToCreate);

        // Assert
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var userFavoriteToCreate = new FakeUserFavoriteForCreation().Generate();
        
        // Act
        var userFavorite = UserFavorite.Create(userFavoriteToCreate);

        // Assert
        userFavorite.DomainEvents.Count.Should().Be(1);
        userFavorite.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserFavoriteCreated));
    }
}