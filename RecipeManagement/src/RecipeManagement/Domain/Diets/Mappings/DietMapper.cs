namespace RecipeManagement.Domain.Diets.Mappings;

using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Domain.Diets.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class DietMapper
{
    public static partial DietForCreation ToDietForCreation(this DietForCreationDto dietForCreationDto);
    public static partial DietForUpdate ToDietForUpdate(this DietForUpdateDto dietForUpdateDto);
    public static partial DietDto ToDietDto(this Diet diet);
    public static partial IQueryable<DietDto> ToDietDtoQueryable(this IQueryable<Diet> diet);
}