namespace RecipeManagement.Domain.UserFavorites.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.UserFavorites.Dtos;
using RecipeManagement.Domain.UserFavorites.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddUserFavorite
{
    public sealed record Command(UserFavoriteForCreationDto UserFavoriteToAdd) : IRequest<UserFavoriteDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, UserFavoriteDto>
    {
        public async Task<UserFavoriteDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userFavoriteToAdd = request.UserFavoriteToAdd.ToUserFavoriteForCreation();
            var userFavorite = UserFavorite.Create(userFavoriteToAdd);

            await dbContext.UserFavorites.AddAsync(userFavorite, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return userFavorite.ToUserFavoriteDto();
        }
    }
}