namespace RecipeManagement.Domain.Ingredients;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.RecipeIngridients;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.Models;


public class Ingredient : BaseEntity
{
    [Required]
    public string Name { get; private set; }

    [Required]
    public decimal Calories { get; private set; }

    [Required]
    public string Unit { get; private set; }

    [Required]
    public decimal Fat { get; private set; }

    [Required]
    public decimal Carbs { get; private set; }

    [Required]
    public decimal Protein { get; private set; }

    [Required]
    public decimal Sugar { get; private set; }

    private readonly List<RecipeIngridient> _recipeIngridients = new();
    public IReadOnlyCollection<RecipeIngridient> RecipeIngridients => _recipeIngridients.AsReadOnly();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Ingredient Create(IngredientForCreation ingredientForCreation)
    {
        var newIngredient = new Ingredient();

        newIngredient.Name = ingredientForCreation.Name;
        newIngredient.Calories = ingredientForCreation.Calories;
        newIngredient.Unit = ingredientForCreation.Unit;
        newIngredient.Fat = ingredientForCreation.Fat;
        newIngredient.Carbs = ingredientForCreation.Carbs;
        newIngredient.Protein = ingredientForCreation.Protein;
        newIngredient.Sugar = ingredientForCreation.Sugar;

        newIngredient.QueueDomainEvent(new IngredientCreated(){ Ingredient = newIngredient });
        
        return newIngredient;
    }

    public Ingredient Update(IngredientForUpdate ingredientForUpdate)
    {
        Name = ingredientForUpdate.Name;
        Calories = ingredientForUpdate.Calories;
        Unit = ingredientForUpdate.Unit;
        Fat = ingredientForUpdate.Fat;
        Carbs = ingredientForUpdate.Carbs;
        Protein = ingredientForUpdate.Protein;
        Sugar = ingredientForUpdate.Sugar;

        QueueDomainEvent(new IngredientUpdated(){ Id = Id });
        return this;
    }

    public Ingredient AddRecipeIngridient(RecipeIngridient recipeIngridient)
    {
        _recipeIngridients.Add(recipeIngridient);
        return this;
    }
    
    public Ingredient RemoveRecipeIngridient(RecipeIngridient recipeIngridient)
    {
        _recipeIngridients.RemoveAll(x => x.Id == recipeIngridient.Id);
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Ingredient() { } // For EF + Mocking
}
