namespace RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;

using AutoBogus;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Models;

public sealed class FakeRecipeIngridientForCreation : AutoFaker<RecipeIngridientForCreation>
{
    public FakeRecipeIngridientForCreation()
    {
    }
}