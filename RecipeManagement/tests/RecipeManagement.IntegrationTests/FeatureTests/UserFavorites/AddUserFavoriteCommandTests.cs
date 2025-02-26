namespace RecipeManagement.IntegrationTests.FeatureTests.UserFavorites;

using RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.UserFavorites.Features;

public class AddUserFavoriteCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_userfavorite_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var userFavoriteOne = new FakeUserFavoriteForCreationDto().Generate();

        // Act
        var command = new AddUserFavorite.Command(userFavoriteOne);
        var userFavoriteReturned = await testingServiceScope.SendAsync(command);
        var userFavoriteCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.UserFavorites
            .FirstOrDefaultAsync(u => u.Id == userFavoriteReturned.Id));

        // Assert

    }
}