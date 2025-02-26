namespace RecipeManagement.Domain.Diets.DomainEvents;

public sealed class DietCreated : DomainEvent
{
    public Diet Diet { get; set; } 
}
            