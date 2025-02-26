namespace RecipeManagement.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateRecipeTests
{
    private readonly Faker _faker;

    public UpdateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_recipe()
    {
        // Arrange
        var recipe = new FakeRecipeBuilder().Build();
        var updatedRecipe = new FakeRecipeForUpdate().Generate();
        
        // Act
        recipe.Update(updatedRecipe);

        // Assert
        recipe.Name.Should().Be(updatedRecipe.Name);
        recipe.ImageUrl.Should().Be(updatedRecipe.ImageUrl);
        recipe.CookingTime.Should().Be(updatedRecipe.CookingTime);
        recipe.Description.Should().Be(updatedRecipe.Description);
        recipe.Instructions.Should().Be(updatedRecipe.Instructions);
        recipe.LikesCount.Should().Be(updatedRecipe.LikesCount);
        recipe.IsDraft.Should().Be(updatedRecipe.IsDraft);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var recipe = new FakeRecipeBuilder().Build();
        var updatedRecipe = new FakeRecipeForUpdate().Generate();
        recipe.DomainEvents.Clear();
        
        // Act
        recipe.Update(updatedRecipe);

        // Assert
        recipe.DomainEvents.Count.Should().Be(1);
        recipe.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeUpdated));
    }
}