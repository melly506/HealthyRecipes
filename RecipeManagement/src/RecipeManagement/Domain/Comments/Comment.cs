namespace RecipeManagement.Domain.Comments;

using System.ComponentModel.DataAnnotations;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Recipes;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain.Comments.Models;
using RecipeManagement.Domain.Comments.DomainEvents;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Models;


public class Comment : BaseEntity
{
    [Required]
    public string Text { get; private set; }

    public Recipe Recipe { get; }

    public User User { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Comment Create(CommentForCreation commentForCreation)
    {
        var newComment = new Comment();

        newComment.Text = commentForCreation.Text;

        newComment.QueueDomainEvent(new CommentCreated(){ Comment = newComment });
        
        return newComment;
    }

    public Comment Update(CommentForUpdate commentForUpdate)
    {
        Text = commentForUpdate.Text;

        QueueDomainEvent(new CommentUpdated(){ Id = Id });
        return this;
    }

    public Comment SetUser(User user)
    {
        User = user;
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Comment() { } // For EF + Mocking
}
