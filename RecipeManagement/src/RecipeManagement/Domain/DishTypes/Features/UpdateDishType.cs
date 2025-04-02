namespace RecipeManagement.Domain.DishTypes.Features;

using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.DishTypes.Models;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class UpdateDishType
{
    public sealed record Command(Guid DishTypeId, DishTypeForUpdateDto UpdatedDishTypeData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var userRoles = await dbContext.UserRoles
                .Include(x => x.User)
                .Where(x => x.User.Identifier == currentUserService.UserId)
                .Select(x => x.Role.Value)
                .ToArrayAsync(cancellationToken);

            var isSuperAdmin = userRoles.Contains(Role.SuperAdmin().Value);

            if (!isSuperAdmin)
            {
                throw new NoRolesAssignedException();
            }
            var dishTypeToUpdate = await dbContext.DishTypes.GetById(request.DishTypeId, cancellationToken: cancellationToken);
            var dishTypeToAdd = request.UpdatedDishTypeData.ToDishTypeForUpdate();
            dishTypeToUpdate.Update(dishTypeToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}