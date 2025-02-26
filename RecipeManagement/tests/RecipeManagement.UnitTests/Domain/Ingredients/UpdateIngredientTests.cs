namespace RecipeManagement.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateIngredientTests
{
    private readonly Faker _faker;

    public UpdateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_ingredient()
    {
        // Arrange
        var ingredient = new FakeIngredientBuilder().Build();
        var updatedIngredient = new FakeIngredientForUpdate().Generate();
        
        // Act
        ingredient.Update(updatedIngredient);

        // Assert
        ingredient.Name.Should().Be(updatedIngredient.Name);
        ingredient.Calories.Should().Be(updatedIngredient.Calories);
        ingredient.Unit.Should().Be(updatedIngredient.Unit);
        ingredient.Fat.Should().Be(updatedIngredient.Fat);
        ingredient.Carbs.Should().Be(updatedIngredient.Carbs);
        ingredient.Protein.Should().Be(updatedIngredient.Protein);
        ingredient.Sugar.Should().Be(updatedIngredient.Sugar);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var ingredient = new FakeIngredientBuilder().Build();
        var updatedIngredient = new FakeIngredientForUpdate().Generate();
        ingredient.DomainEvents.Clear();
        
        // Act
        ingredient.Update(updatedIngredient);

        // Assert
        ingredient.DomainEvents.Count.Should().Be(1);
        ingredient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(IngredientUpdated));
    }
}