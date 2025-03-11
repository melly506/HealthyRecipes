namespace RecipeManagement.Domain.RecipeIngridients.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Domain.RecipeIngridients.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using System.Collections.Generic;

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