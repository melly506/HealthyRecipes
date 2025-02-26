namespace RecipeManagement.UnitTests.Domain.DishTypes;

using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateDishTypeTests
{
    private readonly Faker _faker;

    public UpdateDishTypeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_dishType()
    {
        // Arrange
        var dishType = new FakeDishTypeBuilder().Build();
        var updatedDishType = new FakeDishTypeForUpdate().Generate();
        
        // Act
        dishType.Update(updatedDishType);

        // Assert
        dishType.Name.Should().Be(updatedDishType.Name);
        dishType.Description.Should().Be(updatedDishType.Description);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var dishType = new FakeDishTypeBuilder().Build();
        var updatedDishType = new FakeDishTypeForUpdate().Generate();
        dishType.DomainEvents.Clear();
        
        // Act
        dishType.Update(updatedDishType);

        // Assert
        dishType.DomainEvents.Count.Should().Be(1);
        dishType.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(DishTypeUpdated));
    }
}