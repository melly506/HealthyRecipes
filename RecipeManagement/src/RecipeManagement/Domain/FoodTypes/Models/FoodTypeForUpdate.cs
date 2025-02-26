namespace RecipeManagement.Domain.FoodTypes.Models;

using Destructurama.Attributed;

public sealed record FoodTypeForUpdate
{
    public string Name { get; set; }
    public string Description { get; set; }
}
