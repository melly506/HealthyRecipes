namespace RecipeManagement.Domain.Users;

using RecipeManagement.Exceptions;
using RecipeManagement.Domain.UserFavorites;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.DomainEvents;
using RecipeManagement.Domain.Emails;
using RecipeManagement.Domain.Users.Models;
using Roles;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

public class User : BaseEntity
{
    public string Identifier { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public Email Email { get; private set; }

    public string Username { get; private set; }

    public string Bio { get; private set; }

    public string Gender { get; private set; }

    public string Picture { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public ICollection<UserRole> Roles { get; private set; } = new List<UserRole>();

    public IReadOnlyCollection<Comment> Comments { get; } = new List<Comment>();

    public IReadOnlyCollection<UserFavorite> UserFavorites { get; } = new List<UserFavorite>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static User Create(UserForCreation userForCreation)
    {
        ValidationException.ThrowWhenNullOrWhitespace(userForCreation.Identifier, 
            "Please provide an identifier.");

        var newUser = new User();

        newUser.Identifier = userForCreation.Identifier;
        newUser.FirstName = userForCreation.FirstName;
        newUser.LastName = userForCreation.LastName;
        newUser.Email = new Email(userForCreation.Email);
        newUser.Username = userForCreation.Username;
        newUser.Bio = "";
        newUser.Gender = "";
        newUser.Picture = "";

        newUser.QueueDomainEvent(new UserCreated(){ User = newUser });
        
        return newUser;
    }

    public User Update(UserForUpdate userForUpdate)
    {
        FirstName = userForUpdate.FirstName;
        LastName = userForUpdate.LastName;
        Bio = userForUpdate.Bio;
        Gender = userForUpdate.Gender;
        Picture = userForUpdate.Picture;

        QueueDomainEvent(new UserUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete

    public UserRole AddRole(Role role)
    {
        var newList = Roles.ToList();
        var userRole = UserRole.Create(this, role);
        newList.Add(userRole);
        UpdateRoles(newList);
        return userRole;
    }

    public UserRole RemoveRole(Role role)
    {
        var newList = Roles.ToList();
        var roleToRemove = Roles.FirstOrDefault(x => x.Role == role);
        newList.Remove(roleToRemove);
        UpdateRoles(newList);
        return roleToRemove;
    }

    private void UpdateRoles(IList<UserRole> updates)
    {
        var additions = updates.Where(userRole => Roles.All(x => x.Role != userRole.Role)).ToList();
        var removals = Roles.Where(userRole => updates.All(x => x.Role != userRole.Role)).ToList();
    
        var newList = Roles.ToList();
        removals.ForEach(toRemove => newList.Remove(toRemove));
        additions.ForEach(newRole => newList.Add(newRole));
        Roles = newList;
        QueueDomainEvent(new UserRolesUpdated(){ UserId = Id });
    }
    
    protected User() { } // For EF + Mocking
}
