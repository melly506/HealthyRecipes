namespace RecipeManagement.Domain.RecipeIngridients.Models;

using Destructurama.Attributed;

public sealed record RecipeIngridientForUpdate
{
    public decimal Count { get; set; }
}
