namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Recipes.Features;

public class AddRecipeCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_recipe_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeOne = new FakeRecipeForCreationDto().Generate();

        // Act
        var command = new AddRecipe.Command(recipeOne);
        var recipeReturned = await testingServiceScope.SendAsync(command);
        var recipeCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Recipes
            .FirstOrDefaultAsync(r => r.Id == recipeReturned.Id));

        // Assert
        recipeReturned.Name.Should().Be(recipeOne.Name);
        recipeReturned.ImageUrl.Should().Be(recipeOne.ImageUrl);
        recipeReturned.CookingTime.Should().Be(recipeOne.CookingTime);
        recipeReturned.Description.Should().Be(recipeOne.Description);
        recipeReturned.Instructions.Should().Be(recipeOne.Instructions);
        recipeReturned.LikesCount.Should().Be(recipeOne.LikesCount);
        recipeReturned.IsDraft.Should().Be(recipeOne.IsDraft);

        recipeCreated.Name.Should().Be(recipeOne.Name);
        recipeCreated.ImageUrl.Should().Be(recipeOne.ImageUrl);
        recipeCreated.CookingTime.Should().Be(recipeOne.CookingTime);
        recipeCreated.Description.Should().Be(recipeOne.Description);
        recipeCreated.Instructions.Should().Be(recipeOne.Instructions);
        recipeCreated.LikesCount.Should().Be(recipeOne.LikesCount);
        recipeCreated.IsDraft.Should().Be(recipeOne.IsDraft);
    }
}