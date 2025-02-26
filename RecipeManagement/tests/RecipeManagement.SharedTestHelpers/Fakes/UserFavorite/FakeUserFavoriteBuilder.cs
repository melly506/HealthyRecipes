namespace RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;

using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.Models;

public class FakeUserFavoriteBuilder
{
    private UserFavoriteForCreation _creationData = new FakeUserFavoriteForCreation().Generate();

    public FakeUserFavoriteBuilder WithModel(UserFavoriteForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public UserFavorite Build()
    {
        var result = UserFavorite.Create(_creationData);
        return result;
    }
}