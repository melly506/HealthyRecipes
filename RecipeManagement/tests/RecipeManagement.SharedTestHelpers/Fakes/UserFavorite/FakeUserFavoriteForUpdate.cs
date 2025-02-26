namespace RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;

using AutoBogus;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.Models;

public sealed class FakeUserFavoriteForUpdate : AutoFaker<UserFavoriteForUpdate>
{
    public FakeUserFavoriteForUpdate()
    {
    }
}