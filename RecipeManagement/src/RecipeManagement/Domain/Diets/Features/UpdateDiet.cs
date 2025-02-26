namespace RecipeManagement.Domain.Diets.Features;

using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Diets.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateDiet
{
    public sealed record Command(Guid DietId, DietForUpdateDto UpdatedDietData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var dietToUpdate = await dbContext.Diets.GetById(request.DietId, cancellationToken: cancellationToken);
            var dietToAdd = request.UpdatedDietData.ToDietForUpdate();
            dietToUpdate.Update(dietToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}