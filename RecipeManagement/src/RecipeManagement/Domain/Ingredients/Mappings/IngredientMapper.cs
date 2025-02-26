namespace RecipeManagement.Domain.Ingredients.Mappings;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class IngredientMapper
{
    public static partial IngredientForCreation ToIngredientForCreation(this IngredientForCreationDto ingredientForCreationDto);
    public static partial IngredientForUpdate ToIngredientForUpdate(this IngredientForUpdateDto ingredientForUpdateDto);
    public static partial IngredientDto ToIngredientDto(this Ingredient ingredient);
    public static partial IQueryable<IngredientDto> ToIngredientDtoQueryable(this IQueryable<Ingredient> ingredient);
}