namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;


public static class AttachFoodTypesToRecipe
{
    public sealed record Command(Guid RecipeId, List<Guid> FoodTypeIds) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
    : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes
              .GetById(request.RecipeId, cancellationToken);

            var foodTypes = await dbContext.FoodTypes
                .Where(ft => request.FoodTypeIds.Contains(ft.Id))
                .ToListAsync(cancellationToken);

            foreach (var foodType in foodTypes)
            {
                recipe.AddFoodType(foodType);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
