namespace RecipeManagement.Domain.FoodTypes.DomainEvents;

public sealed class FoodTypeCreated : DomainEvent
{
    public FoodType FoodType { get; set; } 
}
            