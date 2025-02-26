namespace RecipeManagement.Domain.Diets;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Diets.Models;
using RecipeManagement.Domain.Diets.DomainEvents;


public class Diet : BaseEntity
{
    [Required]
    public string Name { get; private set; }

    public string Description { get; private set; }

    public IReadOnlyCollection<Recipe> Recipes { get; } = new List<Recipe>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Diet Create(DietForCreation dietForCreation)
    {
        var newDiet = new Diet();

        newDiet.Name = dietForCreation.Name;
        newDiet.Description = dietForCreation.Description;

        newDiet.QueueDomainEvent(new DietCreated(){ Diet = newDiet });
        
        return newDiet;
    }

    public Diet Update(DietForUpdate dietForUpdate)
    {
        Name = dietForUpdate.Name;
        Description = dietForUpdate.Description;

        QueueDomainEvent(new DietUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Diet() { } // For EF + Mocking
}
