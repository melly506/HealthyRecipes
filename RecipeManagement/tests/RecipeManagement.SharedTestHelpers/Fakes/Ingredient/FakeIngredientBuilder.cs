namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Models;

public class FakeIngredientBuilder
{
    private IngredientForCreation _creationData = new FakeIngredientForCreation().Generate();

    public FakeIngredientBuilder WithModel(IngredientForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeIngredientBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeIngredientBuilder WithCalories(decimal calories)
    {
        _creationData.Calories = calories;
        return this;
    }
    
    public FakeIngredientBuilder WithUnit(string unit)
    {
        _creationData.Unit = unit;
        return this;
    }
    
    public FakeIngredientBuilder WithFat(decimal fat)
    {
        _creationData.Fat = fat;
        return this;
    }
    
    public FakeIngredientBuilder WithCarbs(decimal carbs)
    {
        _creationData.Carbs = carbs;
        return this;
    }
    
    public FakeIngredientBuilder WithProtein(decimal protein)
    {
        _creationData.Protein = protein;
        return this;
    }
    
    public FakeIngredientBuilder WithSugar(decimal sugar)
    {
        _creationData.Sugar = sugar;
        return this;
    }
    
    public Ingredient Build()
    {
        var result = Ingredient.Create(_creationData);
        return result;
    }
}