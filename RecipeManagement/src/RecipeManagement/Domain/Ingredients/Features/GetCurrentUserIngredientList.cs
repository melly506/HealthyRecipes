namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using RecipeManagement.Services;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetCurrentUserIngredientList
{
    public sealed record Query(IngredientParametersDto QueryParameters) : IRequest<PagedList<IngredientDto>>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Query, PagedList<IngredientDto>>
    {
        public async Task<PagedList<IngredientDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Ingredients.AsNoTracking();

            collection = collection.Where(ingredient => ingredient.CreatedBy == currentUserService.UserId);

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