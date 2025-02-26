namespace RecipeManagement.UnitTests.Domain.DishTypes;

using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateDishTypeTests
{
    private readonly Faker _faker;

    public CreateDishTypeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_dishType()
    {
        // Arrange
        var dishTypeToCreate = new FakeDishTypeForCreation().Generate();
        
        // Act
        var dishType = DishType.Create(dishTypeToCreate);

        // Assert
        dishType.Name.Should().Be(dishTypeToCreate.Name);
        dishType.Description.Should().Be(dishTypeToCreate.Description);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var dishTypeToCreate = new FakeDishTypeForCreation().Generate();
        
        // Act
        var dishType = DishType.Create(dishTypeToCreate);

        // Assert
        dishType.DomainEvents.Count.Should().Be(1);
        dishType.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(DishTypeCreated));
    }
}