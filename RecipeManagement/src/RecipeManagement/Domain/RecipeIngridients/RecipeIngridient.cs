namespace RecipeManagement.Domain.RecipeIngridients;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.RecipeIngridients.Models;
using RecipeManagement.Domain.RecipeIngridients.DomainEvents;


public class RecipeIngridient : BaseEntity
{
    public decimal Count { get; private set; }

    public Recipe Recipe { get; }

    public Ingredient Ingredient { get; }

    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static RecipeIngridient Create(RecipeIngridientForCreation recipeIngridientForCreation)
    {
        var newRecipeIngridient = new RecipeIngridient();

        newRecipeIngridient.Count = recipeIngridientForCreation.Count;
        newRecipeIngridient.RecipeId = recipeIngridientForCreation.RecipeId;
        newRecipeIngridient.IngredientId = recipeIngridientForCreation.IngredientId;

        newRecipeIngridient.QueueDomainEvent(new RecipeIngridientCreated(){ RecipeIngridient = newRecipeIngridient });
        
        return newRecipeIngridient;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected RecipeIngridient() { } // For EF + Mocking
}
