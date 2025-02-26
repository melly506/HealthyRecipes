namespace RecipeManagement.SharedTestHelpers.Fakes.Diet;

using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Models;

public class FakeDietBuilder
{
    private DietForCreation _creationData = new FakeDietForCreation().Generate();

    public FakeDietBuilder WithModel(DietForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeDietBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeDietBuilder WithDescription(string description)
    {
        _creationData.Description = description;
        return this;
    }
    
    public Diet Build()
    {
        var result = Diet.Create(_creationData);
        return result;
    }
}