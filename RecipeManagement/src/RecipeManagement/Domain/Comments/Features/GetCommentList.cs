namespace RecipeManagement.Domain.Comments.Features;

using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetCommentList
{
    public sealed record Query(CommentParametersDto QueryParameters, Guid? RecipeId = null) : IRequest<PagedList<CommentDto>>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Query, PagedList<CommentDto>>
    {
        public async Task<PagedList<CommentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Comments
                .Include(c => c.User)
                .AsNoTracking();

            if (request.RecipeId.HasValue)
            {
                collection = collection.Where(c => c.Recipe.Id == request.RecipeId.Value);
            } else
            {
                collection = collection.Where(c => c.User.Identifier.ToString() == currentUserService.UserId);
            }

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