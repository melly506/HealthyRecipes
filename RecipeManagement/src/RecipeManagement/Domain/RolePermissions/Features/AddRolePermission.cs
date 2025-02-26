namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.RolePermissions.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class AddRolePermission
{
    public sealed record Command(RolePermissionForCreationDto RolePermissionToAdd) : IRequest<RolePermissionDto>;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard)
        : IRequestHandler<Command, RolePermissionDto>
    {
        public async Task<RolePermissionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddPermissions);

            var rolePermissionToAdd = request.RolePermissionToAdd.ToRolePermissionForCreation();
            var rolePermission = RolePermission.Create(rolePermissionToAdd);

            await dbContext.RolePermissions.AddAsync(rolePermission, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return rolePermission.ToRolePermissionDto();
        }
    }
}