namespace RecipeManagement.IntegrationTests.FeatureTests.FoodTypes;

using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Domain.FoodTypes.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateFoodTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_foodtype_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var foodType = new FakeFoodTypeBuilder().Build();
        await testingServiceScope.InsertAsync(foodType);
        var updatedFoodTypeDto = new FakeFoodTypeForUpdateDto().Generate();

        // Act
        var command = new UpdateFoodType.Command(foodType.Id, updatedFoodTypeDto);
        await testingServiceScope.SendAsync(command);
        var updatedFoodType = await testingServiceScope
            .ExecuteDbContextAsync(db => db.FoodTypes
                .FirstOrDefaultAsync(f => f.Id == foodType.Id));

        // Assert
        updatedFoodType.Name.Should().Be(updatedFoodTypeDto.Name);
        updatedFoodType.Description.Should().Be(updatedFoodTypeDto.Description);
    }
}