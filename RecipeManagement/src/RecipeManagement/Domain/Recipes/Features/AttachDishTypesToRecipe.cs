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


public static class AttachDishTypesToRecipe
{
    public sealed record Command(Guid RecipeId, List<Guid> DishTypeIds) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
    : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes
              .GetById(request.RecipeId, cancellationToken);

            var dishTypes = await dbContext.DishTypes
                .Where(dishType => request.DishTypeIds.Contains(dishType.Id))
                .ToListAsync(cancellationToken);

            foreach (var dishType in dishTypes)
            {
                recipe.AddDishType(dishType);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
