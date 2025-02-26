namespace RecipeManagement.Domain.Seasons.DomainEvents;

public sealed class SeasonCreated : DomainEvent
{
    public Season Season { get; set; } 
}
            