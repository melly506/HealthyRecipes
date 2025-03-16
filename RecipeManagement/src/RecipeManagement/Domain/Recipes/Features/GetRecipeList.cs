namespace RecipeManagement.Domain.Recipes.Features;

using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryKit;
using QueryKit.Configuration;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Resources;

public static class GetRecipeList
{
    public sealed record Query(RecipeParametersDto QueryParameters) : IRequest<PagedList<RecipeDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<RecipeDto>>
    {
        public async Task<PagedList<RecipeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Recipes.Include(r => r.FoodType).AsNoTracking();

            if (!string.IsNullOrEmpty(request.QueryParameters.FoodTypeId))
            {
                if (Guid.TryParse(request.QueryParameters.FoodTypeId, out var foodTypeId))
                {
                    collection = collection.Where(r => r.FoodType.Any(ft => ft.Id == foodTypeId));
                }
            }

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);

            return await PagedList<RecipeDto>.CreateAsync(appliedCollection.ToRecipeDtoWithFoodTypesQueryable(),
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}