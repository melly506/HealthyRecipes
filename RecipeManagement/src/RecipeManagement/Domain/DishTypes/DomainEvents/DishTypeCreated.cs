namespace RecipeManagement.Domain.DishTypes.DomainEvents;

public sealed class DishTypeCreated : DomainEvent
{
    public DishType DishType { get; set; } 
}
            