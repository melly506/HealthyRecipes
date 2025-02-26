namespace RecipeManagement.UnitTests.Domain.FoodTypes;

using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateFoodTypeTests
{
    private readonly Faker _faker;

    public UpdateFoodTypeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_foodType()
    {
        // Arrange
        var foodType = new FakeFoodTypeBuilder().Build();
        var updatedFoodType = new FakeFoodTypeForUpdate().Generate();
        
        // Act
        foodType.Update(updatedFoodType);

        // Assert
        foodType.Name.Should().Be(updatedFoodType.Name);
        foodType.Description.Should().Be(updatedFoodType.Description);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var foodType = new FakeFoodTypeBuilder().Build();
        var updatedFoodType = new FakeFoodTypeForUpdate().Generate();
        foodType.DomainEvents.Clear();
        
        // Act
        foodType.Update(updatedFoodType);

        // Assert
        foodType.DomainEvents.Count.Should().Be(1);
        foodType.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(FoodTypeUpdated));
    }
}