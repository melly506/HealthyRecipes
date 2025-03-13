namespace RecipeManagement.Domain.RecipeIngridients.Features;

using System.Collections.Generic;
using Mappings;
using MediatR;
using RecipeManagement.Databases;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Dtos;

public static class AddRecipeIngridients
{
    public sealed record Command(List<RecipeIngridientForCreationDto> RecipeIngridientsToAdd) : IRequest<List<RecipeIngridientDto>>;

    public sealed class Handler(RecipesDbContext dbContext) : IRequestHandler<Command, List<RecipeIngridientDto>>
    {
        public async Task<List<RecipeIngridientDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var recipeIngridients = new List<RecipeIngridient>();

            foreach (var recipeIngridientToAdd in request.RecipeIngridientsToAdd)
            {
                var recipeIngridientForCreation = recipeIngridientToAdd.ToRecipeIngridientForCreation();
                var recipeIngridient = RecipeIngridient.Create(recipeIngridientForCreation);
                recipeIngridients.Add(recipeIngridient);
            }

            await dbContext.RecipeIngridients.AddRangeAsync(recipeIngridients, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return recipeIngridients.Select(ri => ri.ToRecipeIngridientDto()).ToList();
        }
    }
}