namespace RecipeManagement.Domain.RecipeIngridients.Dtos;

using Destructurama.Attributed;

public sealed record RecipeIngridientForCreationDto
{
    public decimal Count { get; set; }
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }
}
