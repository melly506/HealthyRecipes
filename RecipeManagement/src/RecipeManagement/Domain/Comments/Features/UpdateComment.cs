namespace RecipeManagement.Domain.Comments.Features;

using Mappings;
using MediatR;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Exceptions;
using RecipeManagement.Services;

public static class UpdateComment
{
    public sealed record Command(Guid CommentId, CommentForUpdateDto UpdatedCommentData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var commentToUpdate = await dbContext.Comments.GetById(request.CommentId, cancellationToken: cancellationToken);
            if (commentToUpdate.CreatedBy != currentUserService.UserId)
            {
                throw new NoRolesAssignedException();
            }
            var commentToAdd = request.UpdatedCommentData.ToCommentForUpdate();
            commentToUpdate.Update(commentToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}