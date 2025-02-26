namespace RecipeManagement.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateIngredientTests
{
    private readonly Faker _faker;

    public CreateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_ingredient()
    {
        // Arrange
        var ingredientToCreate = new FakeIngredientForCreation().Generate();
        
        // Act
        var ingredient = Ingredient.Create(ingredientToCreate);

        // Assert
        ingredient.Name.Should().Be(ingredientToCreate.Name);
        ingredient.Calories.Should().Be(ingredientToCreate.Calories);
        ingredient.Unit.Should().Be(ingredientToCreate.Unit);
        ingredient.Fat.Should().Be(ingredientToCreate.Fat);
        ingredient.Carbs.Should().Be(ingredientToCreate.Carbs);
        ingredient.Protein.Should().Be(ingredientToCreate.Protein);
        ingredient.Sugar.Should().Be(ingredientToCreate.Sugar);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var ingredientToCreate = new FakeIngredientForCreation().Generate();
        
        // Act
        var ingredient = Ingredient.Create(ingredientToCreate);

        // Assert
        ingredient.DomainEvents.Count.Should().Be(1);
        ingredient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(IngredientCreated));
    }
}