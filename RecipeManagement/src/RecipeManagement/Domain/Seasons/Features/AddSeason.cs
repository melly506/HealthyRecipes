namespace RecipeManagement.Domain.Seasons.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Domain.Seasons.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddSeason
{
    public sealed record Command(SeasonForCreationDto SeasonToAdd) : IRequest<SeasonDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, SeasonDto>
    {
        public async Task<SeasonDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var seasonToAdd = request.SeasonToAdd.ToSeasonForCreation();
            var season = Season.Create(seasonToAdd);

            await dbContext.Seasons.AddAsync(season, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return season.ToSeasonDto();
        }
    }
}