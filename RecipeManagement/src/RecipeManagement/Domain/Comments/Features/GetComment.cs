namespace RecipeManagement.Domain.Comments.Features;

using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetComment
{
    public sealed record Query(Guid CommentId) : IRequest<CommentDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, CommentDto>
    {
        public async Task<CommentDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Comments
                .AsNoTracking()
                .GetById(request.CommentId, cancellationToken);
            return result.ToCommentDto();
        }
    }
}