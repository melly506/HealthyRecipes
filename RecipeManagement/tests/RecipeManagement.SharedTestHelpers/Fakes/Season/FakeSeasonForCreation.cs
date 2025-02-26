namespace RecipeManagement.SharedTestHelpers.Fakes.Season;

using AutoBogus;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Models;

public sealed class FakeSeasonForCreation : AutoFaker<SeasonForCreation>
{
    public FakeSeasonForCreation()
    {
    }
}