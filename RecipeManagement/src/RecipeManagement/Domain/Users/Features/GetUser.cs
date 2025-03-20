namespace RecipeManagement.Domain.Users.Features;

using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Users.Dtos;

public static class GetUser
{
    public sealed record Query(Guid UserId) : IRequest<UserDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {

            var result = await dbContext.Users
                .AsNoTracking()
                .GetById(request.UserId, cancellationToken);
            return result.ToUserDto();
        }
    }
}