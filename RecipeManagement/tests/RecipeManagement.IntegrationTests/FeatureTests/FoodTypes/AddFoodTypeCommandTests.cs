namespace RecipeManagement.IntegrationTests.FeatureTests.FoodTypes;

using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.FoodTypes.Features;

public class AddFoodTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_foodtype_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var foodTypeOne = new FakeFoodTypeForCreationDto().Generate();

        // Act
        var command = new AddFoodType.Command(foodTypeOne);
        var foodTypeReturned = await testingServiceScope.SendAsync(command);
        var foodTypeCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.FoodTypes
            .FirstOrDefaultAsync(f => f.Id == foodTypeReturned.Id));

        // Assert
        foodTypeReturned.Name.Should().Be(foodTypeOne.Name);
        foodTypeReturned.Description.Should().Be(foodTypeOne.Description);

        foodTypeCreated.Name.Should().Be(foodTypeOne.Name);
        foodTypeCreated.Description.Should().Be(foodTypeOne.Description);
    }
}