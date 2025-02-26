namespace RecipeManagement.IntegrationTests.FeatureTests.DishTypes;

using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Domain.DishTypes.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateDishTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_dishtype_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dishType = new FakeDishTypeBuilder().Build();
        await testingServiceScope.InsertAsync(dishType);
        var updatedDishTypeDto = new FakeDishTypeForUpdateDto().Generate();

        // Act
        var command = new UpdateDishType.Command(dishType.Id, updatedDishTypeDto);
        await testingServiceScope.SendAsync(command);
        var updatedDishType = await testingServiceScope
            .ExecuteDbContextAsync(db => db.DishTypes
                .FirstOrDefaultAsync(d => d.Id == dishType.Id));

        // Assert
        updatedDishType.Name.Should().Be(updatedDishTypeDto.Name);
        updatedDishType.Description.Should().Be(updatedDishTypeDto.Description);
    }
}