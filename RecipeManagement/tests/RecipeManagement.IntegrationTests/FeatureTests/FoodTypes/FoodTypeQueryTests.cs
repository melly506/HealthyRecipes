namespace RecipeManagement.IntegrationTests.FeatureTests.FoodTypes;

using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using RecipeManagement.Domain.FoodTypes.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class FoodTypeQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_foodtype_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var foodTypeOne = new FakeFoodTypeBuilder().Build();
        await testingServiceScope.InsertAsync(foodTypeOne);

        // Act
        var query = new GetFoodType.Query(foodTypeOne.Id);
        var foodType = await testingServiceScope.SendAsync(query);

        // Assert
        foodType.Name.Should().Be(foodTypeOne.Name);
        foodType.Description.Should().Be(foodTypeOne.Description);
    }

    [Fact]
    public async Task get_foodtype_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetFoodType.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}