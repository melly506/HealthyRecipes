namespace RecipeManagement.IntegrationTests.FeatureTests.UserFavorites;

using RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;
using RecipeManagement.Domain.UserFavorites.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteUserFavoriteCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_userfavorite_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var userFavorite = new FakeUserFavoriteBuilder().Build();
        await testingServiceScope.InsertAsync(userFavorite);

        // Act
        var command = new DeleteUserFavorite.Command(userFavorite.Id);
        await testingServiceScope.SendAsync(command);
        var userFavoriteResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.UserFavorites
                .CountAsync(u => u.Id == userFavorite.Id));

        // Assert
        userFavoriteResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_userfavorite_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteUserFavorite.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_userfavorite_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var userFavorite = new FakeUserFavoriteBuilder().Build();
        await testingServiceScope.InsertAsync(userFavorite);

        // Act
        var command = new DeleteUserFavorite.Command(userFavorite.Id);
        await testingServiceScope.SendAsync(command);
        var deletedUserFavorite = await testingServiceScope.ExecuteDbContextAsync(db => db.UserFavorites
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == userFavorite.Id));

        // Assert
        deletedUserFavorite?.IsDeleted.Should().BeTrue();
    }
}