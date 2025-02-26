namespace RecipeManagement.IntegrationTests.FeatureTests.DishTypes;

using RecipeManagement.SharedTestHelpers.Fakes.DishType;
using RecipeManagement.Domain.DishTypes.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteDishTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_dishtype_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dishType = new FakeDishTypeBuilder().Build();
        await testingServiceScope.InsertAsync(dishType);

        // Act
        var command = new DeleteDishType.Command(dishType.Id);
        await testingServiceScope.SendAsync(command);
        var dishTypeResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.DishTypes
                .CountAsync(d => d.Id == dishType.Id));

        // Assert
        dishTypeResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_dishtype_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteDishType.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_dishtype_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dishType = new FakeDishTypeBuilder().Build();
        await testingServiceScope.InsertAsync(dishType);

        // Act
        var command = new DeleteDishType.Command(dishType.Id);
        await testingServiceScope.SendAsync(command);
        var deletedDishType = await testingServiceScope.ExecuteDbContextAsync(db => db.DishTypes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == dishType.Id));

        // Assert
        deletedDishType?.IsDeleted.Should().BeTrue();
    }
}