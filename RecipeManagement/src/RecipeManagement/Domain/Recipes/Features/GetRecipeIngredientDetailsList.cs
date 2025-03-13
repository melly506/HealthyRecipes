namespace RecipeManagement.Domain.Recipes.Features;

using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;
using RecipeManagement.Domain.RecipeIngridients.Dtos;


public static class GetRecipeIngredientDetailsList
{
    public sealed record Query(Guid RecipeId) : IRequest<List<RecipeIngredientDetailsDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, List<RecipeIngredientDetailsDto>>
    {
        public async Task<List<RecipeIngredientDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var recipeIngredients = await dbContext.RecipeIngridients
                .AsNoTracking()
                .Where(ri => ri.RecipeId == request.RecipeId)
                .Include(ri => ri.Ingredient)
                .ToListAsync(cancellationToken);

            return recipeIngredients
                .Select(ri => new RecipeIngredientDetailsDto
                {
                    Id = ri.Id,
                    Count = ri.Count,
                    RecipeId = ri.RecipeId,
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient.Name,
                    Unit = ri.Ingredient.Unit,
                    Calories = ri.Ingredient.Calories,
                    Fat = ri.Ingredient.Fat,
                    Carbs = ri.Ingredient.Carbs,
                    Protein = ri.Ingredient.Protein,
                    Sugar = ri.Ingredient.Sugar
                })
                .ToList();
        }
    }
}