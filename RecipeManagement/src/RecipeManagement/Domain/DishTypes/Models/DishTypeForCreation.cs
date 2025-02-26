namespace RecipeManagement.Domain.DishTypes.Models;

using Destructurama.Attributed;

public sealed record DishTypeForCreation
{
    public string Name { get; set; }
    public string Description { get; set; }
}
