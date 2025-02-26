namespace RecipeManagement.Domain.Diets.DomainEvents;

public sealed class DietUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            