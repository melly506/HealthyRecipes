namespace RecipeManagement.Domain.RecipeIngridients.Features;

using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using MediatR;

public static class DeleteRecipeIngridient
{
    public sealed record Command(Guid RecipeIngridientId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.RecipeIngridients
                .GetById(request.RecipeIngridientId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}