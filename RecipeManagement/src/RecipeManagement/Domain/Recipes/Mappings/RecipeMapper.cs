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

    public static IQueryable<RecipeDto> ToRecipeDtoWithFoodTypesQueryable(this IQueryable<Recipe> recipes)
    {
        return recipes.Select(r => new RecipeDto
        {
            Id = r.Id,
            Name = r.Name,
            ImageUrl = r.ImageUrl,
            CookingTime = r.CookingTime,
            Description = r.Description,
            Instructions = r.Instructions,
            LikesCount = r.LikesCount,
            IsDraft = r.IsDraft,
            FoodType = r.FoodType.Select(ft => FoodTypeMapper.ToFoodTypeDto(ft)).ToList(),
            CreatedOn = r.CreatedOn,
            CreatedBy = r.CreatedBy,
            LastModifiedOn = r.LastModifiedOn,
            LastModifiedBy = r.LastModifiedBy
        });
    }
}