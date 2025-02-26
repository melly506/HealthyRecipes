namespace RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;

using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Models;

public class FakeRecipeIngridientBuilder
{
    private RecipeIngridientForCreation _creationData = new FakeRecipeIngridientForCreation().Generate();

    public FakeRecipeIngridientBuilder WithModel(RecipeIngridientForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeRecipeIngridientBuilder WithCount(decimal count)
    {
        _creationData.Count = count;
        return this;
    }
    
    public RecipeIngridient Build()
    {
        var result = RecipeIngridient.Create(_creationData);
        return result;
    }
}