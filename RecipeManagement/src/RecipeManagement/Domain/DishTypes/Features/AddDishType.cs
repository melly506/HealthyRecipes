namespace RecipeManagement.Domain.DishTypes.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Domain.DishTypes.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddDishType
{
    public sealed record Command(DishTypeForCreationDto DishTypeToAdd) : IRequest<DishTypeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, DishTypeDto>
    {
        public async Task<DishTypeDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var dishTypeToAdd = request.DishTypeToAdd.ToDishTypeForCreation();
            var dishType = DishType.Create(dishTypeToAdd);

            await dbContext.DishTypes.AddAsync(dishType, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return dishType.ToDishTypeDto();
        }
    }
}