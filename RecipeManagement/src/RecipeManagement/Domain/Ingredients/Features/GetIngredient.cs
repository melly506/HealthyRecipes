namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetIngredient
{
    public sealed record Query(Guid IngredientId) : IRequest<IngredientDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, IngredientDto>
    {
        public async Task<IngredientDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Ingredients
                .AsNoTracking()
                .GetById(request.IngredientId, cancellationToken);
            return result.ToIngredientDto();
        }
    }
}