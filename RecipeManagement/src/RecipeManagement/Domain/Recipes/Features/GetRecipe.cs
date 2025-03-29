namespace RecipeManagement.Domain.Recipes.Features;

using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;
using RecipeManagement.Domain.FoodTypes.Mappings;
using RecipeManagement.Domain.Diets.Mappings;
using RecipeManagement.Domain.Seasons.Mappings;
using RecipeManagement.Domain.DishTypes.Mappings;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Exceptions;
using RecipeManagement.Services;

public static class GetRecipe
{
    public sealed record Query(Guid RecipeId) : IRequest<RecipeDto>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Query, RecipeDto>
    {
        public async Task<RecipeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var recipe = await dbContext.Recipes
                .Include(r => r.UserFavorites)
                .FirstOrDefaultAsync(r => r.Id == request.RecipeId, cancellationToken);

            if (recipe == null)
                throw new NotFoundException(nameof(Recipe), request.RecipeId);

            var foodTypesForRecipe = await dbContext.FoodTypes
                .Where(ft => ft.Recipes.Any(r => r.Id == recipe.Id))
                .Select(ft => FoodTypeMapper.ToFoodTypeDto(ft))
                .ToListAsync(cancellationToken);

            var dietsForRecipe = await dbContext.Diets
                .Where(diet => diet.Recipes.Any(r => r.Id == recipe.Id))
                .Select(diet => DietMapper.ToDietDto(diet))
                .ToListAsync(cancellationToken);

            var seasonsForRecipe = await dbContext.Seasons
                .Where(season => season.Recipes.Any(r => r.Id == recipe.Id))
                .Select(season => SeasonMapper.ToSeasonDto(season))
                .ToListAsync(cancellationToken);

            var dishTypesForRecipe = await dbContext.DishTypes
                .Where(dishType => dishType.Recipes.Any(r => r.Id == recipe.Id))
                .Select(dishType => DishTypeMapper.ToDishTypeDto(dishType))
                .ToListAsync(cancellationToken);

            // Check if current user has liked this recipe
            bool isLiked = false;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                isLiked = await dbContext.UserFavorites
                    .AnyAsync(uf => uf.Recipe.Id == recipe.Id &&
                                  uf.User.Identifier == currentUserId,
                           cancellationToken);
            }

            recipe.SetLikesCount(recipe.UserFavorites?.Count ?? 0);
            var recipeDto = recipe.ToRecipeDto();

            recipeDto.IsLiked = isLiked;
            recipeDto.FoodType = foodTypesForRecipe.ToList();
            recipeDto.Diet = dietsForRecipe.ToList();
            recipeDto.Season = seasonsForRecipe.ToList();
            recipeDto.DishType = dishTypesForRecipe.ToList();

            return recipeDto;
        }
    }
}