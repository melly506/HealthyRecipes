namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class RecipeQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_recipe_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeOne = new FakeRecipeBuilder().Build();
        await testingServiceScope.InsertAsync(recipeOne);

        // Act
        var query = new GetRecipe.Query(recipeOne.Id);
        var recipe = await testingServiceScope.SendAsync(query);

        // Assert
        recipe.Name.Should().Be(recipeOne.Name);
        recipe.ImageUrl.Should().Be(recipeOne.ImageUrl);
        recipe.CookingTime.Should().Be(recipeOne.CookingTime);
        recipe.Description.Should().Be(recipeOne.Description);
        recipe.Instructions.Should().Be(recipeOne.Instructions);
        recipe.LikesCount.Should().Be(recipeOne.LikesCount);
        recipe.IsDraft.Should().Be(recipeOne.IsDraft);
    }

    [Fact]
    public async Task get_recipe_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetRecipe.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}