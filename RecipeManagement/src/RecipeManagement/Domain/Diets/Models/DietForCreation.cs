namespace RecipeManagement.Domain.Diets.Models;

using Destructurama.Attributed;

public sealed record DietForCreation
{
    public string Name { get; set; }
    public string Description { get; set; }
}
