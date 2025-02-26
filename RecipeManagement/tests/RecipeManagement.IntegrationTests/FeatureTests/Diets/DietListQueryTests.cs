namespace RecipeManagement.IntegrationTests.FeatureTests.Diets;

using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using RecipeManagement.Domain.Diets.Features;
using Domain;
using System.Threading.Tasks;

public class DietListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_diet_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dietOne = new FakeDietBuilder().Build();
        var dietTwo = new FakeDietBuilder().Build();
        var queryParameters = new DietParametersDto();

        await testingServiceScope.InsertAsync(dietOne, dietTwo);

        // Act
        var query = new GetDietList.Query(queryParameters);
        var diets = await testingServiceScope.SendAsync(query);

        // Assert
        diets.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}