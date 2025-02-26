namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetIngredientList
{
    public sealed record Query(IngredientParametersDto QueryParameters) : IRequest<PagedList<IngredientDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<IngredientDto>>
    {
        public async Task<PagedList<IngredientDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Ingredients.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToIngredientDtoQueryable();

            return await PagedList<IngredientDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}