namespace RecipeManagement.Domain.Comments.Features;

using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetCommentList
{
    public sealed record Query(CommentParametersDto QueryParameters) : IRequest<PagedList<CommentDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<CommentDto>>
    {
        public async Task<PagedList<CommentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Comments.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToCommentDtoQueryable();

            return await PagedList<CommentDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}