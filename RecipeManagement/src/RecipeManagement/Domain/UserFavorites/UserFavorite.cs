namespace RecipeManagement.Domain.UserFavorites;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.UserFavorites.Models;
using RecipeManagement.Domain.UserFavorites.DomainEvents;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Models;


public class UserFavorite : BaseEntity
{
    public Recipe Recipe { get; }

    public User User { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static UserFavorite Create(UserFavoriteForCreation userFavoriteForCreation)
    {
        var newUserFavorite = new UserFavorite();



        newUserFavorite.QueueDomainEvent(new UserFavoriteCreated(){ UserFavorite = newUserFavorite });
        
        return newUserFavorite;
    }

    public UserFavorite Update(UserFavoriteForUpdate userFavoriteForUpdate)
    {


        QueueDomainEvent(new UserFavoriteUpdated(){ Id = Id });
        return this;
    }

    public UserFavorite SetUser(User user)
    {
        User = user;
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected UserFavorite() { } // For EF + Mocking
}
