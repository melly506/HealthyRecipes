namespace RecipeManagement.Domain.FoodTypes.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Domain.FoodTypes.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddFoodType
{
    public sealed record Command(FoodTypeForCreationDto FoodTypeToAdd) : IRequest<FoodTypeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, FoodTypeDto>
    {
        public async Task<FoodTypeDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var foodTypeToAdd = request.FoodTypeToAdd.ToFoodTypeForCreation();
            var foodType = FoodType.Create(foodTypeToAdd);

            await dbContext.FoodTypes.AddAsync(foodType, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return foodType.ToFoodTypeDto();
        }
    }
}