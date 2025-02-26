namespace RecipeManagement.IntegrationTests.FeatureTests.DishTypes;

using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using RecipeManagement.Domain.DishTypes.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class DishTypeQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_dishtype_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dishTypeOne = new FakeDishTypeBuilder().Build();
        await testingServiceScope.InsertAsync(dishTypeOne);

        // Act
        var query = new GetDishType.Query(dishTypeOne.Id);
        var dishType = await testingServiceScope.SendAsync(query);

        // Assert
        dishType.Name.Should().Be(dishTypeOne.Name);
        dishType.Description.Should().Be(dishTypeOne.Description);
    }

    [Fact]
    public async Task get_dishtype_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetDishType.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}