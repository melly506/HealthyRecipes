namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using HeimGuard;
using Mappings;
using MediatR;
using Roles;

public static class AddUserRole
{
    public sealed record Command(Guid UserId, string Role, bool SkipPermissions = false) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if(!request.SkipPermissions)
                await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUserRoles);
            
            var user = await dbContext.GetUserAggregate().GetById(request.UserId, cancellationToken);

            var roleToAdd = user.AddRole(new Role(request.Role));
            await dbContext.UserRoles.AddAsync(roleToAdd, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}