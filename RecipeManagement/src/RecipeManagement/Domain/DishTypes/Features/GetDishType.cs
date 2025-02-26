namespace RecipeManagement.Domain.DishTypes.Features;

using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetDishType
{
    public sealed record Query(Guid DishTypeId) : IRequest<DishTypeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, DishTypeDto>
    {
        public async Task<DishTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.DishTypes
                .AsNoTracking()
                .GetById(request.DishTypeId, cancellationToken);
            return result.ToDishTypeDto();
        }
    }
}