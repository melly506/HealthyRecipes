namespace RecipeManagement.SharedTestHelpers.Fakes.DishType;

using AutoBogus;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Dtos;

public sealed class FakeDishTypeForUpdateDto : AutoFaker<DishTypeForUpdateDto>
{
    public FakeDishTypeForUpdateDto()
    {
    }
}