namespace RecipeManagement.SharedTestHelpers.Fakes.DishType;

using AutoBogus;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Dtos;

public sealed class FakeDishTypeForCreationDto : AutoFaker<DishTypeForCreationDto>
{
    public FakeDishTypeForCreationDto()
    {
    }
}