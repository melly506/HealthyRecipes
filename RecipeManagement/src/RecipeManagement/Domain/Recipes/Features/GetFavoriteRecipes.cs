namespace RecipeManagement.Domain.Recipes.Features;

using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryKit;
using QueryKit.Configuration;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Resources;
using RecipeManagement.Services;

public static class GetFavoriteRecipeList
{
    public sealed record Query(RecipeParametersDto QueryParameters) : IRequest<PagedList<RecipeDto>>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Query, PagedList<RecipeDto>>
    {
        public async Task<PagedList<RecipeDto>> Handle(Query request, CancellationToken cancellationToken)
        {

            var currentUserId = currentUserService.UserId;

            var collection = dbContext.Recipes
                .Include(r => r.FoodType)
                .Include(r => r.Diet)
                .Include(r => r.Season)
                .Include(r => r.DishType)
                .Include(r => r.UserFavorites)
                .Include(r => r.UserFavorites).ThenInclude(uf => uf.User)
                .AsNoTracking()
                .Where(r => r.UserFavorites.Any(uf => uf.User.Identifier == currentUserId));

            if (!string.IsNullOrEmpty(request.QueryParameters.FoodTypeId))
            {
                if (Guid.TryParse(request.QueryParameters.FoodTypeId, out var foodTypeId))
                {
                    collection = collection.Where(r => r.FoodType.Any(ft => ft.Id == foodTypeId));
                }
            }

            if (!string.IsNullOrEmpty(request.QueryParameters.DietId))
            {
                if (Guid.TryParse(request.QueryParameters.DietId, out var dietId))
                {
                    collection = collection.Where(r => r.Diet.Any(diet => diet.Id == dietId));
                }
            }

            if (!string.IsNullOrEmpty(request.QueryParameters.SeasonId))
            {
                if (Guid.TryParse(request.QueryParameters.SeasonId, out var seasonId))
                {
                    collection = collection.Where(r => r.Season.Any(season => season.Id == seasonId));
                }
            }

            if (!string.IsNullOrEmpty(request.QueryParameters.DishTypeId))
            {
                if (Guid.TryParse(request.QueryParameters.DishTypeId, out var dishTypeId))
                {
                    collection = collection.Where(r => r.DishType.Any(dishType => dishType.Id == dishTypeId));
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

            return await PagedList<RecipeDto>.CreateAsync(appliedCollection.ToRecipeDtoWithChildrenEntitiesQueryable(currentUserId),
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}
