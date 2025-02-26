namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetRecipe
{
    public sealed record Query(Guid RecipeId) : IRequest<RecipeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, RecipeDto>
    {
        public async Task<RecipeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Recipes
                .AsNoTracking()
                .GetById(request.RecipeId, cancellationToken);
            return result.ToRecipeDto();
        }
    }
}