namespace RecipeManagement.SharedTestHelpers.Fakes.FoodType;

using AutoBogus;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Models;

public sealed class FakeFoodTypeForCreation : AutoFaker<FoodTypeForCreation>
{
    public FakeFoodTypeForCreation()
    {
    }
}