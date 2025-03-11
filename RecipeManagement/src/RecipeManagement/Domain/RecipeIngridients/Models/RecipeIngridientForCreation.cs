namespace RecipeManagement.Domain.RecipeIngridients.Models;

using Destructurama.Attributed;

public sealed record RecipeIngridientForCreation
{
    public decimal Count { get; set; }
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }
}
