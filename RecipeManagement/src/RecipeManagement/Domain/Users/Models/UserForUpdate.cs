namespace RecipeManagement.Domain.Users.Models;

using Destructurama.Attributed;

public sealed record UserForUpdate
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
    public string Gender { get; set; }
    public string Picture { get; set; }
}
