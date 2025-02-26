namespace RecipeManagement.Domain.Seasons.DomainEvents;

public sealed class SeasonUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            