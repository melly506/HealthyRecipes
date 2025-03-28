namespace RecipeManagement.Domain.Recipes.Features;

using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.Models;
using RecipeManagement.Services;


public static class AddLikeToRecipe
{
    public sealed record Command(Guid RecipeId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Identifier == currentUserService.UserId, cancellationToken);
            var recipe = await dbContext.Recipes
              .GetById(request.RecipeId, cancellationToken);

            var existingLike = await dbContext.UserFavorites
                .FirstOrDefaultAsync(uf =>
                    uf.User.Id == user.Id &&
                    uf.Recipe.Id == recipe.Id,
                    cancellationToken);


            if (existingLike != null)
            {
                throw new InvalidOperationException("alreadyLiked");
            }

            var userFavoriteForCreation = new UserFavoriteForCreation();
            var userFavorite = UserFavorite.Create(userFavoriteForCreation);


            userFavorite.SetUser(user);
            recipe.AddUserFavorite(userFavorite);

            await dbContext.UserFavorites.AddAsync(userFavorite, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
