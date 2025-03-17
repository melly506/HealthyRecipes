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


public static class AttachDietsToRecipe
{
    public sealed record Command(Guid RecipeId, List<Guid> DietIds) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
    : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes
              .GetById(request.RecipeId, cancellationToken);

            var diets = await dbContext.Diets
                .Where(diet => request.DietIds.Contains(diet.Id))
                .ToListAsync(cancellationToken);

            foreach (var diet in diets)
            {
                recipe.AddDiet(diet);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
