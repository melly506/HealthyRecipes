namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Users.Models;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class UpdateUser
{
    public sealed record Command(Guid UserId, UserForUpdateDto UpdatedUserData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateUsers);

            var userToUpdate = await dbContext.Users.GetById(request.UserId, cancellationToken: cancellationToken);
            var userToAdd = request.UpdatedUserData.ToUserForUpdate();
            userToUpdate.Update(userToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}