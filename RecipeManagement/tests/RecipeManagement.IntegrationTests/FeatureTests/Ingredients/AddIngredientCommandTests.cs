namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Ingredients.Features;

public class AddIngredientCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_ingredient_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredientOne = new FakeIngredientForCreationDto().Generate();

        // Act
        var command = new AddIngredient.Command(ingredientOne);
        var ingredientReturned = await testingServiceScope.SendAsync(command);
        var ingredientCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredientReturned.Id));

        // Assert
        ingredientReturned.Name.Should().Be(ingredientOne.Name);
        ingredientReturned.Calories.Should().BeApproximately(ingredientOne.Calories, 0.001M);
        ingredientReturned.Unit.Should().Be(ingredientOne.Unit);
        ingredientReturned.Fat.Should().BeApproximately(ingredientOne.Fat, 0.001M);
        ingredientReturned.Carbs.Should().BeApproximately(ingredientOne.Carbs, 0.001M);
        ingredientReturned.Protein.Should().BeApproximately(ingredientOne.Protein, 0.001M);
        ingredientReturned.Sugar.Should().BeApproximately(ingredientOne.Sugar, 0.001M);

        ingredientCreated.Name.Should().Be(ingredientOne.Name);
        ingredientCreated.Calories.Should().BeApproximately(ingredientOne.Calories, 0.001M);
        ingredientCreated.Unit.Should().Be(ingredientOne.Unit);
        ingredientCreated.Fat.Should().BeApproximately(ingredientOne.Fat, 0.001M);
        ingredientCreated.Carbs.Should().BeApproximately(ingredientOne.Carbs, 0.001M);
        ingredientCreated.Protein.Should().BeApproximately(ingredientOne.Protein, 0.001M);
        ingredientCreated.Sugar.Should().BeApproximately(ingredientOne.Sugar, 0.001M);
    }
}