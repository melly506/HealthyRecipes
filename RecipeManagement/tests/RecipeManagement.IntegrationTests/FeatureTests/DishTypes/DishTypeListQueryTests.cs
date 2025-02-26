namespace RecipeManagement.IntegrationTests.FeatureTests.DishTypes;

using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using RecipeManagement.Domain.DishTypes.Features;
using Domain;
using System.Threading.Tasks;

public class DishTypeListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_dishtype_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dishTypeOne = new FakeDishTypeBuilder().Build();
        var dishTypeTwo = new FakeDishTypeBuilder().Build();
        var queryParameters = new DishTypeParametersDto();

        await testingServiceScope.InsertAsync(dishTypeOne, dishTypeTwo);

        // Act
        var query = new GetDishTypeList.Query(queryParameters);
        var dishTypes = await testingServiceScope.SendAsync(query);

        // Assert
        dishTypes.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}