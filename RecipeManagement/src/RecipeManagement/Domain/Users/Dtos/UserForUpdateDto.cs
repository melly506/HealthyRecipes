namespace RecipeManagement.Domain.Users.Dtos;

using Destructurama.Attributed;

public sealed record UserForUpdateDto
{
    public string Identifier { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }

}
