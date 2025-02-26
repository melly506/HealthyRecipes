namespace RecipeManagement.Domain.FoodTypes.Mappings;

using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Domain.FoodTypes.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class FoodTypeMapper
{
    public static partial FoodTypeForCreation ToFoodTypeForCreation(this FoodTypeForCreationDto foodTypeForCreationDto);
    public static partial FoodTypeForUpdate ToFoodTypeForUpdate(this FoodTypeForUpdateDto foodTypeForUpdateDto);
    public static partial FoodTypeDto ToFoodTypeDto(this FoodType foodType);
    public static partial IQueryable<FoodTypeDto> ToFoodTypeDtoQueryable(this IQueryable<FoodType> foodType);
}