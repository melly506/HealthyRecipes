namespace RecipeManagement.Domain.DishTypes.Features;

using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.DishTypes.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateDishType
{
    public sealed record Command(Guid DishTypeId, DishTypeForUpdateDto UpdatedDishTypeData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var dishTypeToUpdate = await dbContext.DishTypes.GetById(request.DishTypeId, cancellationToken: cancellationToken);
            var dishTypeToAdd = request.UpdatedDishTypeData.ToDishTypeForUpdate();
            dishTypeToUpdate.Update(dishTypeToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}