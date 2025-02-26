namespace RecipeManagement.Domain.Seasons;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Seasons.Models;
using RecipeManagement.Domain.Seasons.DomainEvents;


public class Season : BaseEntity
{
    [Required]
    public string Name { get; private set; }

    public IReadOnlyCollection<Recipe> Recipes { get; } = new List<Recipe>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Season Create(SeasonForCreation seasonForCreation)
    {
        var newSeason = new Season();

        newSeason.Name = seasonForCreation.Name;

        newSeason.QueueDomainEvent(new SeasonCreated(){ Season = newSeason });
        
        return newSeason;
    }

    public Season Update(SeasonForUpdate seasonForUpdate)
    {
        Name = seasonForUpdate.Name;

        QueueDomainEvent(new SeasonUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Season() { } // For EF + Mocking
}
