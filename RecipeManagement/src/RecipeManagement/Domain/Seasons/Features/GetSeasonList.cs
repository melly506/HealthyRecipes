namespace RecipeManagement.Domain.Seasons.Features;

using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetSeasonList
{
    public sealed record Query(SeasonParametersDto QueryParameters) : IRequest<PagedList<SeasonDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<SeasonDto>>
    {
        public async Task<PagedList<SeasonDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Seasons.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToSeasonDtoQueryable();

            return await PagedList<SeasonDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}