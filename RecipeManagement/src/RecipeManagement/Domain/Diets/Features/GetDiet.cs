namespace RecipeManagement.Domain.Diets.Features;

using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetDiet
{
    public sealed record Query(Guid DietId) : IRequest<DietDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, DietDto>
    {
        public async Task<DietDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Diets
                .AsNoTracking()
                .GetById(request.DietId, cancellationToken);
            return result.ToDietDto();
        }
    }
}