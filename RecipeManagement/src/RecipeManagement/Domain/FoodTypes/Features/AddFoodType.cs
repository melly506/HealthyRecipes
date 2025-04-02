namespace RecipeManagement.Domain.FoodTypes.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Domain.FoodTypes.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Mappings;
using MediatR;

public static class AddFoodType
{
    public sealed record Command(FoodTypeForCreationDto FoodTypeToAdd) : IRequest<FoodTypeDto>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command, FoodTypeDto>
    {
        public async Task<FoodTypeDto> Handle(Command request, CancellationToken cancellationToken)
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
            var foodTypeToAdd = request.FoodTypeToAdd.ToFoodTypeForCreation();
            var foodType = FoodType.Create(foodTypeToAdd);

            await dbContext.FoodTypes.AddAsync(foodType, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return foodType.ToFoodTypeDto();
        }
    }
}