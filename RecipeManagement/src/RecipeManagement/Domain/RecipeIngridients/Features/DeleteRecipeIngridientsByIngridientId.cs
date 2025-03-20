namespace RecipeManagement.Domain.RecipeIngridients.Features;

using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;

public static class DeleteRecipeIngridientsByIngridientId
{
    public sealed record Command(Guid IngredientId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordsToDelete = await dbContext.RecipeIngridients
                .Where(ri => ri.IngredientId == request.IngredientId)
                .ToListAsync(cancellationToken);

            if (recordsToDelete.Any())
            {
                dbContext.RemoveRange(recordsToDelete);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
