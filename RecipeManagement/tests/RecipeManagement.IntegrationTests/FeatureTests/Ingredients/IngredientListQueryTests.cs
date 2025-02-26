namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Features;
using Domain;
using System.Threading.Tasks;

public class IngredientListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_ingredient_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredientOne = new FakeIngredientBuilder().Build();
        var ingredientTwo = new FakeIngredientBuilder().Build();
        var queryParameters = new IngredientParametersDto();

        await testingServiceScope.InsertAsync(ingredientOne, ingredientTwo);

        // Act
        var query = new GetIngredientList.Query(queryParameters);
        var ingredients = await testingServiceScope.SendAsync(query);

        // Assert
        ingredients.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}