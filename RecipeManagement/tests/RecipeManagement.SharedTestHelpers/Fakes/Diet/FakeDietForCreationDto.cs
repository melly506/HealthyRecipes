namespace RecipeManagement.SharedTestHelpers.Fakes.Diet;

using AutoBogus;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Dtos;

public sealed class FakeDietForCreationDto : AutoFaker<DietForCreationDto>
{
    public FakeDietForCreationDto()
    {
    }
}