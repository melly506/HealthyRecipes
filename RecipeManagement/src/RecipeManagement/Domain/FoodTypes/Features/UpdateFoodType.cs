namespace RecipeManagement.Domain.FoodTypes.Features;

using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.FoodTypes.Models;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class UpdateFoodType
{
    public sealed record Command(Guid FoodTypeId, FoodTypeForUpdateDto UpdatedFoodTypeData) : IRequest;

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
            var foodTypeToUpdate = await dbContext.FoodTypes.GetById(request.FoodTypeId, cancellationToken: cancellationToken);
            var foodTypeToAdd = request.UpdatedFoodTypeData.ToFoodTypeForUpdate();
            foodTypeToUpdate.Update(foodTypeToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}