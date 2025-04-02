namespace RecipeManagement.Domain.Diets.Features;

using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Diets.Models;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class UpdateDiet
{
    public sealed record Command(Guid DietId, DietForUpdateDto UpdatedDietData) : IRequest;

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

            var dietToUpdate = await dbContext.Diets.GetById(request.DietId, cancellationToken: cancellationToken);
            var dietToAdd = request.UpdatedDietData.ToDietForUpdate();
            dietToUpdate.Update(dietToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}