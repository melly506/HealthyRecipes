namespace RecipeManagement.Domain.Seasons.Features;

using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetSeason
{
    public sealed record Query(Guid SeasonId) : IRequest<SeasonDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, SeasonDto>
    {
        public async Task<SeasonDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Seasons
                .AsNoTracking()
                .GetById(request.SeasonId, cancellationToken);
            return result.ToSeasonDto();
        }
    }
}