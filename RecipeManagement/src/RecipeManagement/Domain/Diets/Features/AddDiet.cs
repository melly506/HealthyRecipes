namespace RecipeManagement.Domain.Diets.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Domain.Diets.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class AddDiet
{
    public sealed record Command(DietForCreationDto DietToAdd) : IRequest<DietDto>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command, DietDto>
    {
        public async Task<DietDto> Handle(Command request, CancellationToken cancellationToken)
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


            var dietToAdd = request.DietToAdd.ToDietForCreation();
            var diet = Diet.Create(dietToAdd);

            await dbContext.Diets.AddAsync(diet, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return diet.ToDietDto();
        }
    }
}