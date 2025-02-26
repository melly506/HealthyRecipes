namespace RecipeManagement.Domain.Diets.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Domain.Diets.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddDiet
{
    public sealed record Command(DietForCreationDto DietToAdd) : IRequest<DietDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, DietDto>
    {
        public async Task<DietDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var dietToAdd = request.DietToAdd.ToDietForCreation();
            var diet = Diet.Create(dietToAdd);

            await dbContext.Diets.AddAsync(diet, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return diet.ToDietDto();
        }
    }
}