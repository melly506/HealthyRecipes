namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.RolePermissions.Models;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class UpdateRolePermission
{
    public sealed record Command(Guid RolePermissionId, RolePermissionForUpdateDto UpdatedRolePermissionData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdatePermissions);

            var rolePermissionToUpdate = await dbContext.RolePermissions.GetById(request.RolePermissionId, cancellationToken: cancellationToken);
            var rolePermissionToAdd = request.UpdatedRolePermissionData.ToRolePermissionForUpdate();
            rolePermissionToUpdate.Update(rolePermissionToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}