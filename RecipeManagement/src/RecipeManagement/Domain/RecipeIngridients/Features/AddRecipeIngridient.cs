namespace RecipeManagement.Domain.RecipeIngridients.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Domain.RecipeIngridients.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddRecipeIngridient
{
    public sealed record Command(RecipeIngridientForCreationDto RecipeIngridientToAdd) : IRequest<RecipeIngridientDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, RecipeIngridientDto>
    {
        public async Task<RecipeIngridientDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var recipeIngridientToAdd = request.RecipeIngridientToAdd.ToRecipeIngridientForCreation();
            var recipeIngridient = RecipeIngridient.Create(recipeIngridientToAdd);

            await dbContext.RecipeIngridients.AddAsync(recipeIngridient, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return recipeIngridient.ToRecipeIngridientDto();
        }
    }
}