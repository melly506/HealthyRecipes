namespace RecipeManagement.Domain.DishTypes.Mappings;

using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Domain.DishTypes.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class DishTypeMapper
{
    public static partial DishTypeForCreation ToDishTypeForCreation(this DishTypeForCreationDto dishTypeForCreationDto);
    public static partial DishTypeForUpdate ToDishTypeForUpdate(this DishTypeForUpdateDto dishTypeForUpdateDto);
    public static partial DishTypeDto ToDishTypeDto(this DishType dishType);
    public static partial IQueryable<DishTypeDto> ToDishTypeDtoQueryable(this IQueryable<DishType> dishType);
}