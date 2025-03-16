namespace RecipeManagement.Domain.Recipes.Features;

using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;
using RecipeManagement.Domain.FoodTypes.Mappings;
using RecipeManagement.Domain.Recipes.Dtos;

public static class GetRecipe
{
    public sealed record Query(Guid RecipeId) : IRequest<RecipeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, RecipeDto>
    {
        public async Task<RecipeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes
                .FirstOrDefaultAsync(r => r.Id == request.RecipeId, cancellationToken);

            var foodTypesForRecipe = await dbContext.FoodTypes
                .Where(ft => ft.Recipes.Any(r => r.Id == recipe.Id))
                .Select(ft => FoodTypeMapper.ToFoodTypeDto(ft))
                .ToListAsync(cancellationToken);

            var recipeDto = recipe.ToRecipeDto();

            recipeDto.FoodType = foodTypesForRecipe.ToList();

            return recipeDto;
        }
    }
}