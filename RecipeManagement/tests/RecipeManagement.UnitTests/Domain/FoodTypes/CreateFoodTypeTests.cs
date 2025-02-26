namespace RecipeManagement.UnitTests.Domain.FoodTypes;

using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateFoodTypeTests
{
    private readonly Faker _faker;

    public CreateFoodTypeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_foodType()
    {
        // Arrange
        var foodTypeToCreate = new FakeFoodTypeForCreation().Generate();
        
        // Act
        var foodType = FoodType.Create(foodTypeToCreate);

        // Assert
        foodType.Name.Should().Be(foodTypeToCreate.Name);
        foodType.Description.Should().Be(foodTypeToCreate.Description);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var foodTypeToCreate = new FakeFoodTypeForCreation().Generate();
        
        // Act
        var foodType = FoodType.Create(foodTypeToCreate);

        // Assert
        foodType.DomainEvents.Count.Should().Be(1);
        foodType.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(FoodTypeCreated));
    }
}