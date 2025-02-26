namespace RecipeManagement.SharedTestHelpers.Fakes.Season;

using AutoBogus;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Dtos;

public sealed class FakeSeasonForUpdateDto : AutoFaker<SeasonForUpdateDto>
{
    public FakeSeasonForUpdateDto()
    {
    }
}