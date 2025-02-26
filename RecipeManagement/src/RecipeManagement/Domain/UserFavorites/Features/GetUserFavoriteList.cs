namespace RecipeManagement.Domain.UserFavorites.Features;

using RecipeManagement.Domain.UserFavorites.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetUserFavoriteList
{
    public sealed record Query(UserFavoriteParametersDto QueryParameters) : IRequest<PagedList<UserFavoriteDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<UserFavoriteDto>>
    {
        public async Task<PagedList<UserFavoriteDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.UserFavorites.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToUserFavoriteDtoQueryable();

            return await PagedList<UserFavoriteDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}