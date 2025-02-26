namespace RecipeManagement.Domain.Seasons.Mappings;

using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Domain.Seasons.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class SeasonMapper
{
    public static partial SeasonForCreation ToSeasonForCreation(this SeasonForCreationDto seasonForCreationDto);
    public static partial SeasonForUpdate ToSeasonForUpdate(this SeasonForUpdateDto seasonForUpdateDto);
    public static partial SeasonDto ToSeasonDto(this Season season);
    public static partial IQueryable<SeasonDto> ToSeasonDtoQueryable(this IQueryable<Season> season);
}