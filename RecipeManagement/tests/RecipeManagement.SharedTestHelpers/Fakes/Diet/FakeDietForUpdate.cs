namespace RecipeManagement.SharedTestHelpers.Fakes.Diet;

using AutoBogus;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Models;

public sealed class FakeDietForUpdate : AutoFaker<DietForUpdate>
{
    public FakeDietForUpdate()
    {
    }
}