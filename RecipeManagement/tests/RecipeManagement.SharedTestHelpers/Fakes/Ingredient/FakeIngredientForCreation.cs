namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using AutoBogus;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Models;

public sealed class FakeIngredientForCreation : AutoFaker<IngredientForCreation>
{
    public FakeIngredientForCreation()
    {
    }
}