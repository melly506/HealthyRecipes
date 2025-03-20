namespace RecipeManagement.Domain.Users.Features;

using Domain.Roles;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.Models;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using RecipeManagement.Services;
using HeimGuard;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

public static class GetOrCreateCurrentUser
{
    public sealed record Query() : IRequest<UserDto>;

    public sealed class Handler(
        RecipesDbContext dbContext, 
        ICurrentUserService currentUserService,
        ILogger logger
    )
        : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {

            logger.Information("Fetching current user details for UserId: {UserId}", currentUserService.UserId);

            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Identifier == currentUserService.UserId, cancellationToken);

            if (user == null)
            {

                logger.Information("User not exist in db, creating: {Email}", currentUserService.Email);

                var userToAdd = new UserForCreationDto()
                {
                    Identifier = currentUserService.UserId,
                    FirstName = currentUserService.FirstName,
                    LastName = currentUserService.LastName,
                    Username = currentUserService.Username,
                    Email = currentUserService.Email,
                    Bio = "",
                    Gender = "",
                    Picture = ""
                };

                var newUser = User.Create(userToAdd.ToUserForCreation());
                await dbContext.Users.AddAsync(newUser, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                var userAdded = await dbContext.Users
                    .Include(u => u.Roles)
                    .GetById(newUser.Id, cancellationToken);

                logger.Information("User added to db successfully:", newUser.Id);


                var roleToAdd = newUser.AddRole(Role.User());
                await dbContext.UserRoles.AddAsync(roleToAdd, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                logger.Information("User role assigned successfully:");

                return userAdded.ToUserDto();

            }

            return user.ToUserDto();
        }
    }
}