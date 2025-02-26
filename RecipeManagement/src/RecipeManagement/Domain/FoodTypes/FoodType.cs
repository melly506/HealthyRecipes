namespace RecipeManagement.Domain.FoodTypes;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.FoodTypes.Models;
using RecipeManagement.Domain.FoodTypes.DomainEvents;


public class FoodType : BaseEntity
{
    [Required]
    public string Name { get; private set; }

    public string Description { get; private set; }

    public IReadOnlyCollection<Recipe> Recipes { get; } = new List<Recipe>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static FoodType Create(FoodTypeForCreation foodTypeForCreation)
    {
        var newFoodType = new FoodType();

        newFoodType.Name = foodTypeForCreation.Name;
        newFoodType.Description = foodTypeForCreation.Description;

        newFoodType.QueueDomainEvent(new FoodTypeCreated(){ FoodType = newFoodType });
        
        return newFoodType;
    }

    public FoodType Update(FoodTypeForUpdate foodTypeForUpdate)
    {
        Name = foodTypeForUpdate.Name;
        Description = foodTypeForUpdate.Description;

        QueueDomainEvent(new FoodTypeUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected FoodType() { } // For EF + Mocking
}
