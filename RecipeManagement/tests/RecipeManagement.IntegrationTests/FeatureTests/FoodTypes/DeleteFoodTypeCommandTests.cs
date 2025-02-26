namespace RecipeManagement.IntegrationTests.FeatureTests.FoodTypes;

using RecipeManagement.SharedTestHelpers.Fakes.FoodType;
using RecipeManagement.Domain.FoodTypes.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteFoodTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_foodtype_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var foodType = new FakeFoodTypeBuilder().Build();
        await testingServiceScope.InsertAsync(foodType);

        // Act
        var command = new DeleteFoodType.Command(foodType.Id);
        await testingServiceScope.SendAsync(command);
        var foodTypeResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.FoodTypes
                .CountAsync(f => f.Id == foodType.Id));

        // Assert
        foodTypeResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_foodtype_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteFoodType.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_foodtype_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var foodType = new FakeFoodTypeBuilder().Build();
        await testingServiceScope.InsertAsync(foodType);

        // Act
        var command = new DeleteFoodType.Command(foodType.Id);
        await testingServiceScope.SendAsync(command);
        var deletedFoodType = await testingServiceScope.ExecuteDbContextAsync(db => db.FoodTypes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == foodType.Id));

        // Assert
        deletedFoodType?.IsDeleted.Should().BeTrue();
    }
}