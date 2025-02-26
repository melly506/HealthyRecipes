namespace RecipeManagement.IntegrationTests.FeatureTests.Diets;

using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using RecipeManagement.Domain.Diets.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteDietCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_diet_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var diet = new FakeDietBuilder().Build();
        await testingServiceScope.InsertAsync(diet);

        // Act
        var command = new DeleteDiet.Command(diet.Id);
        await testingServiceScope.SendAsync(command);
        var dietResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Diets
                .CountAsync(d => d.Id == diet.Id));

        // Assert
        dietResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_diet_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteDiet.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_diet_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var diet = new FakeDietBuilder().Build();
        await testingServiceScope.InsertAsync(diet);

        // Act
        var command = new DeleteDiet.Command(diet.Id);
        await testingServiceScope.SendAsync(command);
        var deletedDiet = await testingServiceScope.ExecuteDbContextAsync(db => db.Diets
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == diet.Id));

        // Assert
        deletedDiet?.IsDeleted.Should().BeTrue();
    }
}