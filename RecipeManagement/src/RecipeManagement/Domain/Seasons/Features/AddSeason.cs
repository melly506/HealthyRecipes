namespace RecipeManagement.Domain.Seasons.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Domain.Seasons.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class AddSeason
{
    public sealed record Command(SeasonForCreationDto SeasonToAdd) : IRequest<SeasonDto>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command, SeasonDto>
    {
        public async Task<SeasonDto> Handle(Command request, CancellationToken cancellationToken)
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

            var seasonToAdd = request.SeasonToAdd.ToSeasonForCreation();
            var season = Season.Create(seasonToAdd);

            await dbContext.Seasons.AddAsync(season, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return season.ToSeasonDto();
        }
    }
}