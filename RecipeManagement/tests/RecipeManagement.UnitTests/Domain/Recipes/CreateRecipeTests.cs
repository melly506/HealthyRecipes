namespace RecipeManagement.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateRecipeTests
{
    private readonly Faker _faker;

    public CreateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_recipe()
    {
        // Arrange
        var recipeToCreate = new FakeRecipeForCreation().Generate();
        
        // Act
        var recipe = Recipe.Create(recipeToCreate);

        // Assert
        recipe.Name.Should().Be(recipeToCreate.Name);
        recipe.ImageUrl.Should().Be(recipeToCreate.ImageUrl);
        recipe.CookingTime.Should().Be(recipeToCreate.CookingTime);
        recipe.Description.Should().Be(recipeToCreate.Description);
        recipe.Instructions.Should().Be(recipeToCreate.Instructions);
        recipe.LikesCount.Should().Be(recipeToCreate.LikesCount);
        recipe.IsDraft.Should().Be(recipeToCreate.IsDraft);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var recipeToCreate = new FakeRecipeForCreation().Generate();
        
        // Act
        var recipe = Recipe.Create(recipeToCreate);

        // Assert
        recipe.DomainEvents.Count.Should().Be(1);
        recipe.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeCreated));
    }
}