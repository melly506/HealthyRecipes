namespace RecipeManagement.SharedTestHelpers.Fakes.Season;

using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Models;

public class FakeSeasonBuilder
{
    private SeasonForCreation _creationData = new FakeSeasonForCreation().Generate();

    public FakeSeasonBuilder WithModel(SeasonForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeSeasonBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public Season Build()
    {
        var result = Season.Create(_creationData);
        return result;
    }
}