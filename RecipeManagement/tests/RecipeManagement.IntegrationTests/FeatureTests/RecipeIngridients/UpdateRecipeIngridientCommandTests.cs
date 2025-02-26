namespace RecipeManagement.IntegrationTests.FeatureTests.RecipeIngridients;

using RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Domain.RecipeIngridients.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateRecipeIngridientCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_recipeingridient_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeIngridient = new FakeRecipeIngridientBuilder().Build();
        await testingServiceScope.InsertAsync(recipeIngridient);
        var updatedRecipeIngridientDto = new FakeRecipeIngridientForUpdateDto().Generate();

        // Act
        var command = new UpdateRecipeIngridient.Command(recipeIngridient.Id, updatedRecipeIngridientDto);
        await testingServiceScope.SendAsync(command);
        var updatedRecipeIngridient = await testingServiceScope
            .ExecuteDbContextAsync(db => db.RecipeIngridients
                .FirstOrDefaultAsync(r => r.Id == recipeIngridient.Id));

        // Assert
        updatedRecipeIngridient.Count.Should().BeApproximately(updatedRecipeIngridientDto.Count, 0.001M);
    }
}