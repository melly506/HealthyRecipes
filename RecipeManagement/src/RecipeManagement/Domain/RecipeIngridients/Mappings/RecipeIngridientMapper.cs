namespace RecipeManagement.Domain.RecipeIngridients.Mappings;

using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Domain.RecipeIngridients.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class RecipeIngridientMapper
{
    public static partial RecipeIngridientForCreation ToRecipeIngridientForCreation(this RecipeIngridientForCreationDto recipeIngridientForCreationDto);
    public static partial RecipeIngridientForUpdate ToRecipeIngridientForUpdate(this RecipeIngridientForUpdateDto recipeIngridientForUpdateDto);
    public static partial RecipeIngridientDto ToRecipeIngridientDto(this RecipeIngridient recipeIngridient);
    public static partial IQueryable<RecipeIngridientDto> ToRecipeIngridientDtoQueryable(this IQueryable<RecipeIngridient> recipeIngridient);
}