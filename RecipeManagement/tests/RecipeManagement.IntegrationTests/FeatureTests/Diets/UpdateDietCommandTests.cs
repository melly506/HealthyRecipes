namespace RecipeManagement.IntegrationTests.FeatureTests.Diets;

using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Domain.Diets.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateDietCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_diet_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var diet = new FakeDietBuilder().Build();
        await testingServiceScope.InsertAsync(diet);
        var updatedDietDto = new FakeDietForUpdateDto().Generate();

        // Act
        var command = new UpdateDiet.Command(diet.Id, updatedDietDto);
        await testingServiceScope.SendAsync(command);
        var updatedDiet = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Diets
                .FirstOrDefaultAsync(d => d.Id == diet.Id));

        // Assert
        updatedDiet.Name.Should().Be(updatedDietDto.Name);
        updatedDiet.Description.Should().Be(updatedDietDto.Description);
    }
}