namespace RecipeManagement.SharedTestHelpers.Fakes.UserFavorite;

using AutoBogus;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.Dtos;

public sealed class FakeUserFavoriteForCreationDto : AutoFaker<UserFavoriteForCreationDto>
{
    public FakeUserFavoriteForCreationDto()
    {
    }
}