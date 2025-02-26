namespace RecipeManagement.IntegrationTests.FeatureTests.RecipeIngridients;

using RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;
using RecipeManagement.Domain.RecipeIngridients.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteRecipeIngridientCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_recipeingridient_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeIngridient = new FakeRecipeIngridientBuilder().Build();
        await testingServiceScope.InsertAsync(recipeIngridient);

        // Act
        var command = new DeleteRecipeIngridient.Command(recipeIngridient.Id);
        await testingServiceScope.SendAsync(command);
        var recipeIngridientResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.RecipeIngridients
                .CountAsync(r => r.Id == recipeIngridient.Id));

        // Assert
        recipeIngridientResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_recipeingridient_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteRecipeIngridient.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_recipeingridient_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeIngridient = new FakeRecipeIngridientBuilder().Build();
        await testingServiceScope.InsertAsync(recipeIngridient);

        // Act
        var command = new DeleteRecipeIngridient.Command(recipeIngridient.Id);
        await testingServiceScope.SendAsync(command);
        var deletedRecipeIngridient = await testingServiceScope.ExecuteDbContextAsync(db => db.RecipeIngridients
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == recipeIngridient.Id));

        // Assert
        deletedRecipeIngridient?.IsDeleted.Should().BeTrue();
    }
}