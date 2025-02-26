namespace RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;

using AutoBogus;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Dtos;

public sealed class FakeRecipeIngridientForUpdateDto : AutoFaker<RecipeIngridientForUpdateDto>
{
    public FakeRecipeIngridientForUpdateDto()
    {
    }
}