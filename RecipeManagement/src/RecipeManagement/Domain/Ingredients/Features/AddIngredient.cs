namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddIngredient
{
    public sealed record Command(IngredientForCreationDto IngredientToAdd) : IRequest<IngredientDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, IngredientDto>
    {
        public async Task<IngredientDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var ingredientToAdd = request.IngredientToAdd.ToIngredientForCreation();
            var ingredient = Ingredient.Create(ingredientToAdd);

            await dbContext.Ingredients.AddAsync(ingredient, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return ingredient.ToIngredientDto();
        }
    }
}