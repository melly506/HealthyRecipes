namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateIngredientCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_ingredient_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredient = new FakeIngredientBuilder().Build();
        await testingServiceScope.InsertAsync(ingredient);
        var updatedIngredientDto = new FakeIngredientForUpdateDto().Generate();

        // Act
        var command = new UpdateIngredient.Command(ingredient.Id, updatedIngredientDto);
        await testingServiceScope.SendAsync(command);
        var updatedIngredient = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Ingredients
                .FirstOrDefaultAsync(i => i.Id == ingredient.Id));

        // Assert
        updatedIngredient.Name.Should().Be(updatedIngredientDto.Name);
        updatedIngredient.Calories.Should().BeApproximately(updatedIngredientDto.Calories, 0.001M);
        updatedIngredient.Unit.Should().Be(updatedIngredientDto.Unit);
        updatedIngredient.Fat.Should().BeApproximately(updatedIngredientDto.Fat, 0.001M);
        updatedIngredient.Carbs.Should().BeApproximately(updatedIngredientDto.Carbs, 0.001M);
        updatedIngredient.Protein.Should().BeApproximately(updatedIngredientDto.Protein, 0.001M);
        updatedIngredient.Sugar.Should().BeApproximately(updatedIngredientDto.Sugar, 0.001M);
    }
}