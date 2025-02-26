namespace RecipeManagement.IntegrationTests.FeatureTests.Diets;

using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Diets.Features;

public class AddDietCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_diet_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var dietOne = new FakeDietForCreationDto().Generate();

        // Act
        var command = new AddDiet.Command(dietOne);
        var dietReturned = await testingServiceScope.SendAsync(command);
        var dietCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Diets
            .FirstOrDefaultAsync(d => d.Id == dietReturned.Id));

        // Assert
        dietReturned.Name.Should().Be(dietOne.Name);
        dietReturned.Description.Should().Be(dietOne.Description);

        dietCreated.Name.Should().Be(dietOne.Name);
        dietCreated.Description.Should().Be(dietOne.Description);
    }
}