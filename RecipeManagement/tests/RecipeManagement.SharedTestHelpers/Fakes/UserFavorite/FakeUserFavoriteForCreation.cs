namespace RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;

using AutoBogus;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.Models;

public sealed class FakeUserFavoriteForCreation : AutoFaker<UserFavoriteForCreation>
{
    public FakeUserFavoriteForCreation()
    {
    }
}