namespace RecipeManagement.SharedTestHelpers.Fakes.DishType;

using AutoBogus;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Models;

public sealed class FakeDishTypeForUpdate : AutoFaker<DishTypeForUpdate>
{
    public FakeDishTypeForUpdate()
    {
    }
}