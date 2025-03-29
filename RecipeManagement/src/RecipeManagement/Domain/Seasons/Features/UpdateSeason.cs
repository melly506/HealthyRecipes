namespace RecipeManagement.Domain.Seasons.Features;

using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Seasons.Models;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class UpdateSeason
{
    public sealed record Command(Guid SeasonId, SeasonForUpdateDto UpdatedSeasonData) : IRequest;

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

            var seasonToUpdate = await dbContext.Seasons.GetById(request.SeasonId, cancellationToken: cancellationToken);
            var seasonToAdd = request.UpdatedSeasonData.ToSeasonForUpdate();
            seasonToUpdate.Update(seasonToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}