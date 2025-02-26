namespace RecipeManagement.Domain.UserFavorites.Mappings;

using RecipeManagement.Domain.UserFavorites.Dtos;
using RecipeManagement.Domain.UserFavorites.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class UserFavoriteMapper
{
    public static partial UserFavoriteForCreation ToUserFavoriteForCreation(this UserFavoriteForCreationDto userFavoriteForCreationDto);
    public static partial UserFavoriteForUpdate ToUserFavoriteForUpdate(this UserFavoriteForUpdateDto userFavoriteForUpdateDto);
    public static partial UserFavoriteDto ToUserFavoriteDto(this UserFavorite userFavorite);
    public static partial IQueryable<UserFavoriteDto> ToUserFavoriteDtoQueryable(this IQueryable<UserFavorite> userFavorite);
}