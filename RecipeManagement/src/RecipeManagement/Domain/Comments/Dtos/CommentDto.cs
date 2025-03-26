namespace RecipeManagement.Domain.Comments.Dtos;

using RecipeManagement.Domain.Users.Dtos;

using Destructurama.Attributed;

public sealed record CommentDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string RecipeId { get; set; }
    public UserDto User { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }

}
