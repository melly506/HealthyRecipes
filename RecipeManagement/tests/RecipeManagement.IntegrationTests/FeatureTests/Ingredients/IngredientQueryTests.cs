namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class IngredientQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_ingredient_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredientOne = new FakeIngredientBuilder().Build();
        await testingServiceScope.InsertAsync(ingredientOne);

        // Act
        var query = new GetIngredient.Query(ingredientOne.Id);
        var ingredient = await testingServiceScope.SendAsync(query);

        // Assert
        ingredient.Name.Should().Be(ingredientOne.Name);
        ingredient.Calories.Should().BeApproximately(ingredientOne.Calories, 0.001M);
        ingredient.Unit.Should().Be(ingredientOne.Unit);
        ingredient.Fat.Should().BeApproximately(ingredientOne.Fat, 0.001M);
        ingredient.Carbs.Should().BeApproximately(ingredientOne.Carbs, 0.001M);
        ingredient.Protein.Should().BeApproximately(ingredientOne.Protein, 0.001M);
        ingredient.Sugar.Should().BeApproximately(ingredientOne.Sugar, 0.001M);
    }

    [Fact]
    public async Task get_ingredient_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetIngredient.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}