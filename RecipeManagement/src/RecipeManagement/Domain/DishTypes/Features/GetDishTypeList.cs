namespace RecipeManagement.Domain.DishTypes.Features;

using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetDishTypeList
{
    public sealed record Query(DishTypeParametersDto QueryParameters) : IRequest<PagedList<DishTypeDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<DishTypeDto>>
    {
        public async Task<PagedList<DishTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.DishTypes.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToDishTypeDtoQueryable();

            return await PagedList<DishTypeDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}