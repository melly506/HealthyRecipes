namespace RecipeManagement.Domain.RecipeIngridients.DomainEvents;

public sealed class RecipeIngridientCreated : DomainEvent
{
    public RecipeIngridient RecipeIngridient { get; set; } 
}
            