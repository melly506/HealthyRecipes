namespace RecipeManagement.Domain.Seasons.Features;

using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Seasons.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateSeason
{
    public sealed record Command(Guid SeasonId, SeasonForUpdateDto UpdatedSeasonData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var seasonToUpdate = await dbContext.Seasons.GetById(request.SeasonId, cancellationToken: cancellationToken);
            var seasonToAdd = request.UpdatedSeasonData.ToSeasonForUpdate();
            seasonToUpdate.Update(seasonToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}