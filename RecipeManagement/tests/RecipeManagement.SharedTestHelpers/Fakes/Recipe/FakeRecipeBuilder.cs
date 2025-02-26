namespace RecipeManagement.SharedTestHelpers.Fakes.Recipe;

using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Models;

public class FakeRecipeBuilder
{
    private RecipeForCreation _creationData = new FakeRecipeForCreation().Generate();

    public FakeRecipeBuilder WithModel(RecipeForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeRecipeBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeRecipeBuilder WithImageUrl(string imageUrl)
    {
        _creationData.ImageUrl = imageUrl;
        return this;
    }
    
    public FakeRecipeBuilder WithCookingTime(int cookingTime)
    {
        _creationData.CookingTime = cookingTime;
        return this;
    }
    
    public FakeRecipeBuilder WithDescription(string description)
    {
        _creationData.Description = description;
        return this;
    }
    
    public FakeRecipeBuilder WithInstructions(string instructions)
    {
        _creationData.Instructions = instructions;
        return this;
    }
    
    public FakeRecipeBuilder WithLikesCount(int likesCount)
    {
        _creationData.LikesCount = likesCount;
        return this;
    }
    
    public FakeRecipeBuilder WithIsDraft(bool isDraft)
    {
        _creationData.IsDraft = isDraft;
        return this;
    }
    
    public Recipe Build()
    {
        var result = Recipe.Create(_creationData);
        return result;
    }
}