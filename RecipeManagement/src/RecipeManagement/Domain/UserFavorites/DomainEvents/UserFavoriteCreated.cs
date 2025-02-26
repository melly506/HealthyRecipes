namespace RecipeManagement.Domain.UserFavorites.DomainEvents;

public sealed class UserFavoriteCreated : DomainEvent
{
    public UserFavorite UserFavorite { get; set; } 
}
            