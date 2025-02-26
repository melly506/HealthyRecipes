namespace RecipeManagement.Domain.Comments.Features;

using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Comments.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateComment
{
    public sealed record Command(Guid CommentId, CommentForUpdateDto UpdatedCommentData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var commentToUpdate = await dbContext.Comments.GetById(request.CommentId, cancellationToken: cancellationToken);
            var commentToAdd = request.UpdatedCommentData.ToCommentForUpdate();
            commentToUpdate.Update(commentToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}