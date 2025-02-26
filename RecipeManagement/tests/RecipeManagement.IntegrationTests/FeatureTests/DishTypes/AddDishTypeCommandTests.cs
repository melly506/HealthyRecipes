namespace RecipeManagement.IntegrationTests.FeatureTests.DishTypes;

using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.DishTypes.Features;

public class AddDishTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_dishtype_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dishTypeOne = new FakeDishTypeForCreationDto().Generate();

        // Act
        var command = new AddDishType.Command(dishTypeOne);
        var dishTypeReturned = await testingServiceScope.SendAsync(command);
        var dishTypeCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.DishTypes
            .FirstOrDefaultAsync(d => d.Id == dishTypeReturned.Id));

        // Assert
        dishTypeReturned.Name.Should().Be(dishTypeOne.Name);
        dishTypeReturned.Description.Should().Be(dishTypeOne.Description);

        dishTypeCreated.Name.Should().Be(dishTypeOne.Name);
        dishTypeCreated.Description.Should().Be(dishTypeOne.Description);
    }
}