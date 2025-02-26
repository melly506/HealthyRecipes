namespace RecipeManagement.SharedTestHelpers.Fakes.Season;

using AutoBogus;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Models;

public sealed class FakeSeasonForUpdate : AutoFaker<SeasonForUpdate>
{
    public FakeSeasonForUpdate()
    {
    }
}