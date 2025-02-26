namespace RecipeManagement.IntegrationTests.FeatureTests.FoodTypes;

using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using RecipeManagement.Domain.FoodTypes.Features;
using Domain;
using System.Threading.Tasks;

public class FoodTypeListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_foodtype_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var foodTypeOne = new FakeFoodTypeBuilder().Build();
        var foodTypeTwo = new FakeFoodTypeBuilder().Build();
        var queryParameters = new FoodTypeParametersDto();

        await testingServiceScope.InsertAsync(foodTypeOne, foodTypeTwo);

        // Act
        var query = new GetFoodTypeList.Query(queryParameters);
        var foodTypes = await testingServiceScope.SendAsync(query);

        // Assert
        foodTypes.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}