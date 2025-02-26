namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateIngredient
{
    public sealed record Command(Guid IngredientId, IngredientForUpdateDto UpdatedIngredientData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var ingredientToUpdate = await dbContext.Ingredients.GetById(request.IngredientId, cancellationToken: cancellationToken);
            var ingredientToAdd = request.UpdatedIngredientData.ToIngredientForUpdate();
            ingredientToUpdate.Update(ingredientToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}