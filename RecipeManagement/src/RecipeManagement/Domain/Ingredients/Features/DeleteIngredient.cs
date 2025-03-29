namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using MediatR;

public static class DeleteIngredient
{
    public sealed record Command(Guid IngredientId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.Ingredients
                .GetById(request.IngredientId, cancellationToken: cancellationToken);
            if (recordToDelete.CreatedBy != currentUserService.UserId)
            {
                throw new NoRolesAssignedException();
            }

            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}