namespace RecipeManagement.SharedTestHelpers.Fakes.FoodType;

using AutoBogus;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Dtos;

public sealed class FakeFoodTypeForCreationDto : AutoFaker<FoodTypeForCreationDto>
{
    public FakeFoodTypeForCreationDto()
    {
    }
}