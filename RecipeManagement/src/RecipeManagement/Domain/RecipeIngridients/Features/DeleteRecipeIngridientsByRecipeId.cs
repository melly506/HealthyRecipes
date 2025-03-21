namespace RecipeManagement.Domain.RecipeIngridients.Features;

using MediatR;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Databases;

public static class DeleteRecipeIngridientsByRecipeId
{
    public sealed record Command(Guid RecipeId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext): IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordsToDelete = await dbContext.RecipeIngridients
                .Where(ri => ri.RecipeId == request.RecipeId)
                .ToListAsync(cancellationToken);

            if (recordsToDelete.Any())
            {
                dbContext.RemoveRange(recordsToDelete);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
