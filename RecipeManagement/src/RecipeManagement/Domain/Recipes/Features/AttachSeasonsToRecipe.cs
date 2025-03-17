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


public static class AttachSeasonsToRecipe
{
    public sealed record Command(Guid RecipeId, List<Guid> SeasonIds) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
    : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes
              .GetById(request.RecipeId, cancellationToken);

            var seasons = await dbContext.Seasons
                .Where(season => request.SeasonIds.Contains(season.Id))
                .ToListAsync(cancellationToken);

            foreach (var season in seasons)
            {
                recipe.AddSeason(season);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
