namespace RecipeManagement.Domain.UserFavorites.DomainEvents;

public sealed class UserFavoriteUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            