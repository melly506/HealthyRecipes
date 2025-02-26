namespace RecipeManagement.Domain.RecipeIngridients.Features;

using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.RecipeIngridients.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateRecipeIngridient
{
    public sealed record Command(Guid RecipeIngridientId, RecipeIngridientForUpdateDto UpdatedRecipeIngridientData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipeIngridientToUpdate = await dbContext.RecipeIngridients.GetById(request.RecipeIngridientId, cancellationToken: cancellationToken);
            var recipeIngridientToAdd = request.UpdatedRecipeIngridientData.ToRecipeIngridientForUpdate();
            recipeIngridientToUpdate.Update(recipeIngridientToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}