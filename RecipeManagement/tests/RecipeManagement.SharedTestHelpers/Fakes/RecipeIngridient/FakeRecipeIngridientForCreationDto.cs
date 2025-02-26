namespace RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;

using AutoBogus;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Dtos;

public sealed class FakeRecipeIngridientForCreationDto : AutoFaker<RecipeIngridientForCreationDto>
{
    public FakeRecipeIngridientForCreationDto()
    {
    }
}