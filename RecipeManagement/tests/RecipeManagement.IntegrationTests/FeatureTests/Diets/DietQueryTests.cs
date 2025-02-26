namespace RecipeManagement.IntegrationTests.FeatureTests.Diets;

using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using RecipeManagement.Domain.Diets.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class DietQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_diet_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dietOne = new FakeDietBuilder().Build();
        await testingServiceScope.InsertAsync(dietOne);

        // Act
        var query = new GetDiet.Query(dietOne.Id);
        var diet = await testingServiceScope.SendAsync(query);

        // Assert
        diet.Name.Should().Be(dietOne.Name);
        diet.Description.Should().Be(dietOne.Description);
    }

    [Fact]
    public async Task get_diet_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetDiet.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}