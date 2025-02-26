namespace RecipeManagement.SharedTestHelpers.Fakes.DishType;

using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Models;

public class FakeDishTypeBuilder
{
    private DishTypeForCreation _creationData = new FakeDishTypeForCreation().Generate();

    public FakeDishTypeBuilder WithModel(DishTypeForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeDishTypeBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeDishTypeBuilder WithDescription(string description)
    {
        _creationData.Description = description;
        return this;
    }
    
    public DishType Build()
    {
        var result = DishType.Create(_creationData);
        return result;
    }
}