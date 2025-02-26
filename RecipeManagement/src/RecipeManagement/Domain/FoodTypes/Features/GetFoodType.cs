namespace RecipeManagement.Domain.FoodTypes.Features;

using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetFoodType
{
    public sealed record Query(Guid FoodTypeId) : IRequest<FoodTypeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, FoodTypeDto>
    {
        public async Task<FoodTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.FoodTypes
                .AsNoTracking()
                .GetById(request.FoodTypeId, cancellationToken);
            return result.ToFoodTypeDto();
        }
    }
}