namespace RecipeManagement.Domain.RecipeIngridients.Models;

using Destructurama.Attributed;

public sealed record RecipeIngridientForCreation
{
    public decimal Count { get; set; }
}
