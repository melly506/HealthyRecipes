namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class AddUser
{
    public sealed record Command(UserForCreationDto UserToAdd, bool SkipPermissions = false) : IRequest<UserDto>;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard)
        : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            if(!request.SkipPermissions)
                await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUsers);

            var userToAdd = request.UserToAdd.ToUserForCreation();
            var user = User.Create(userToAdd);
            await dbContext.Users.AddAsync(user, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            var userAdded = await dbContext.Users.GetById(user.Id, cancellationToken);
            return userAdded.ToUserDto();
        }
    }
}
