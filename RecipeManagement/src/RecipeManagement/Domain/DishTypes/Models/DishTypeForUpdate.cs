namespace RecipeManagement.Domain.DishTypes.Models;

using Destructurama.Attributed;

public sealed record DishTypeForUpdate
{
    public string Name { get; set; }
    public string Description { get; set; }
}
