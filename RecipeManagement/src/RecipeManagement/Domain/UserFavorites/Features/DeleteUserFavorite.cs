namespace RecipeManagement.Domain.UserFavorites.Features;

using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using MediatR;

public static class DeleteUserFavorite
{
    public sealed record Command(Guid UserFavoriteId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.UserFavorites
                .GetById(request.UserFavoriteId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}