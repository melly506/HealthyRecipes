namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Recipes.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateRecipe
{
    public sealed record Command(Guid RecipeId, RecipeForUpdateDto UpdatedRecipeData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipeToUpdate = await dbContext.Recipes.GetById(request.RecipeId, cancellationToken: cancellationToken);
            var recipeToAdd = request.UpdatedRecipeData.ToRecipeForUpdate();
            recipeToUpdate.Update(recipeToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}