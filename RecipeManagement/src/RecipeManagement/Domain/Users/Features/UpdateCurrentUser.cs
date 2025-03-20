namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Users.Models;
using RecipeManagement.Exceptions;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class UpdateCurrentUser
{
	public sealed record Command(UserForUpdateDto UpdatedUserData) : IRequest;

	public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
		: IRequestHandler<Command>
	{
		public async Task Handle(Command request, CancellationToken cancellationToken)
		{

			var userToUpdate = await dbContext.Users
				.FirstOrDefaultAsync(u => u.Identifier == currentUserService.UserId, cancellationToken);
			var userToAdd = request.UpdatedUserData.ToUserForUpdate();
			userToUpdate.Update(userToAdd);

			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}