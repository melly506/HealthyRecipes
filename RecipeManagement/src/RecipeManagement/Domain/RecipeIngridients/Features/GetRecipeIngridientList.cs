namespace RecipeManagement.Domain.RecipeIngridients.Features;

using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetRecipeIngridientList
{
    public sealed record Query(RecipeIngridientParametersDto QueryParameters) : IRequest<PagedList<RecipeIngridientDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<RecipeIngridientDto>>
    {
        public async Task<PagedList<RecipeIngridientDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.RecipeIngridients.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToRecipeIngridientDtoQueryable();

            return await PagedList<RecipeIngridientDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}