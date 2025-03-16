namespace RecipeManagement.Domain.Recipes.Mappings;

using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Models;
using RecipeManagement.Domain.FoodTypes.Mappings;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class RecipeMapper
{
    public static partial RecipeForCreation ToRecipeForCreation(this RecipeForCreationDto recipeForCreationDto);
    public static partial RecipeForUpdate ToRecipeForUpdate(this RecipeForUpdateDto recipeForUpdateDto);
    [MapperIgnoreTarget(nameof(RecipeDto.FoodType))]
    public static partial RecipeDto ToRecipeDto(this Recipe recipe);
    public static partial IQueryable<RecipeDto> ToRecipeDtoQueryable(this IQueryable<Recipe> recipe);
}