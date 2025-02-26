namespace RecipeManagement.SharedTestHelpers.Fakes.Diet;

using AutoBogus;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Dtos;

public sealed class FakeDietForUpdateDto : AutoFaker<DietForUpdateDto>
{
    public FakeDietForUpdateDto()
    {
    }
}