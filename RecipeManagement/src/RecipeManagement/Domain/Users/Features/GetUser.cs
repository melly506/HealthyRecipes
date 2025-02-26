namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetUser
{
    public sealed record Query(Guid UserId) : IRequest<UserDto>;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard)
        : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanGetUsers);

            var result = await dbContext.Users
                .AsNoTracking()
                .GetById(request.UserId, cancellationToken);
            return result.ToUserDto();
        }
    }
}