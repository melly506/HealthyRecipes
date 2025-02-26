namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteIngredientCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_ingredient_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredient = new FakeIngredientBuilder().Build();
        await testingServiceScope.InsertAsync(ingredient);

        // Act
        var command = new DeleteIngredient.Command(ingredient.Id);
        await testingServiceScope.SendAsync(command);
        var ingredientResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Ingredients
                .CountAsync(i => i.Id == ingredient.Id));

        // Assert
        ingredientResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_ingredient_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteIngredient.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_ingredient_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredient = new FakeIngredientBuilder().Build();
        await testingServiceScope.InsertAsync(ingredient);

        // Act
        var command = new DeleteIngredient.Command(ingredient.Id);
        await testingServiceScope.SendAsync(command);
        var deletedIngredient = await testingServiceScope.ExecuteDbContextAsync(db => db.Ingredients
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == ingredient.Id));

        // Assert
        deletedIngredient?.IsDeleted.Should().BeTrue();
    }
}