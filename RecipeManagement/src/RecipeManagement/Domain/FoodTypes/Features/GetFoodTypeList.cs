namespace RecipeManagement.Domain.FoodTypes.Features;

using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetFoodTypeList
{
    public sealed record Query(FoodTypeParametersDto QueryParameters) : IRequest<PagedList<FoodTypeDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<FoodTypeDto>>
    {
        public async Task<PagedList<FoodTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.FoodTypes.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToFoodTypeDtoQueryable();

            return await PagedList<FoodTypeDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}