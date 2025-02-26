namespace RecipeManagement.SharedTestHelpers.Fakes.FoodType;

using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Models;

public class FakeFoodTypeBuilder
{
    private FoodTypeForCreation _creationData = new FakeFoodTypeForCreation().Generate();

    public FakeFoodTypeBuilder WithModel(FoodTypeForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeFoodTypeBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeFoodTypeBuilder WithDescription(string description)
    {
        _creationData.Description = description;
        return this;
    }
    
    public FoodType Build()
    {
        var result = FoodType.Create(_creationData);
        return result;
    }
}