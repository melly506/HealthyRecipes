namespace RecipeManagement.Domain.FoodTypes.DomainEvents;

public sealed class FoodTypeUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            