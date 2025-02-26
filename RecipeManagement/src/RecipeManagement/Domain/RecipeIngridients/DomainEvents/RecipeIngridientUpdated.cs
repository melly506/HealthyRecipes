namespace RecipeManagement.Domain.RecipeIngridients.DomainEvents;

public sealed class RecipeIngridientUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            