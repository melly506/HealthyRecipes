namespace RecipeManagement.Domain.DishTypes;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.DishTypes.Models;
using RecipeManagement.Domain.DishTypes.DomainEvents;


public class DishType : BaseEntity
{
    [Required]
    public string Name { get; private set; }

    public string Description { get; private set; }

    public IReadOnlyCollection<Recipe> Recipes { get; } = new List<Recipe>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static DishType Create(DishTypeForCreation dishTypeForCreation)
    {
        var newDishType = new DishType();

        newDishType.Name = dishTypeForCreation.Name;
        newDishType.Description = dishTypeForCreation.Description;

        newDishType.QueueDomainEvent(new DishTypeCreated(){ DishType = newDishType });
        
        return newDishType;
    }

    public DishType Update(DishTypeForUpdate dishTypeForUpdate)
    {
        Name = dishTypeForUpdate.Name;
        Description = dishTypeForUpdate.Description;

        QueueDomainEvent(new DishTypeUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected DishType() { } // For EF + Mocking
}
