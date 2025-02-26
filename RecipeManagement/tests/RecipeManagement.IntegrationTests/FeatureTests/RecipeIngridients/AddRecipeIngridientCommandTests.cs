namespace RecipeManagement.IntegrationTests.FeatureTests.RecipeIngridients;

using RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.RecipeIngridients.Features;

public class AddRecipeIngridientCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_recipeingridient_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeIngridientOne = new FakeRecipeIngridientForCreationDto().Generate();

        // Act
        var command = new AddRecipeIngridient.Command(recipeIngridientOne);
        var recipeIngridientReturned = await testingServiceScope.SendAsync(command);
        var recipeIngridientCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.RecipeIngridients
            .FirstOrDefaultAsync(r => r.Id == recipeIngridientReturned.Id));

        // Assert
        recipeIngridientReturned.Count.Should().BeApproximately(recipeIngridientOne.Count, 0.001M);

        recipeIngridientCreated.Count.Should().BeApproximately(recipeIngridientOne.Count, 0.001M);
    }
}