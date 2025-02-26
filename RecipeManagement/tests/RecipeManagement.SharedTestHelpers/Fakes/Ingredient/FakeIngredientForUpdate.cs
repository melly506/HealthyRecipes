namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using AutoBogus;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Models;

public sealed class FakeIngredientForUpdate : AutoFaker<IngredientForUpdate>
{
    public FakeIngredientForUpdate()
    {
    }
}