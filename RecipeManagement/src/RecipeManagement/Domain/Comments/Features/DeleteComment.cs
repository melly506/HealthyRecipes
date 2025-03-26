namespace RecipeManagement.Domain.Comments.Features;

using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Microsoft.EntityFrameworkCore;
using MediatR;

public static class DeleteComment
{
    public sealed record Command(Guid CommentId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {

            var commentToDelete = await dbContext.Comments
                .Include(c => c.Recipe)
                .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

            if (commentToDelete == null)
            {
                throw new NotFoundException("Comment not found.");
            }

            var currentUserId = currentUserService.UserId;

            if (commentToDelete.CreatedBy != currentUserId &&
                commentToDelete.Recipe.CreatedBy != currentUserId)
            {
                throw new NoRolesAssignedException();
            }

            dbContext.Remove(commentToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}