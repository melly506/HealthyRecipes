namespace RecipeManagement.SharedTestHelpers.Fakes.DishType;

using AutoBogus;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Models;

public sealed class FakeDishTypeForCreation : AutoFaker<DishTypeForCreation>
{
    public FakeDishTypeForCreation()
    {
    }
}