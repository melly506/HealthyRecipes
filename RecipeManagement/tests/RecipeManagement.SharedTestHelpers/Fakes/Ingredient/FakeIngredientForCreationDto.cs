namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using AutoBogus;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;

public sealed class FakeIngredientForCreationDto : AutoFaker<IngredientForCreationDto>
{
    public FakeIngredientForCreationDto()
    {
    }
}