namespace RecipeManagement.Domain.DishTypes.DomainEvents;

public sealed class DishTypeUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            