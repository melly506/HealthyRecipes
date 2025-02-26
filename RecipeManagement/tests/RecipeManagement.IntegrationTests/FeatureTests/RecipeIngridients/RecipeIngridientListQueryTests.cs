namespace RecipeManagement.IntegrationTests.FeatureTests.RecipeIngridients;

using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;
using RecipeManagement.Domain.RecipeIngridients.Features;
using Domain;
using System.Threading.Tasks;

public class RecipeIngridientListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_recipeingridient_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeIngridientOne = new FakeRecipeIngridientBuilder().Build();
        var recipeIngridientTwo = new FakeRecipeIngridientBuilder().Build();
        var queryParameters = new RecipeIngridientParametersDto();

        await testingServiceScope.InsertAsync(recipeIngridientOne, recipeIngridientTwo);

        // Act
        var query = new GetRecipeIngridientList.Query(queryParameters);
        var recipeIngridients = await testingServiceScope.SendAsync(query);

        // Assert
        recipeIngridients.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}