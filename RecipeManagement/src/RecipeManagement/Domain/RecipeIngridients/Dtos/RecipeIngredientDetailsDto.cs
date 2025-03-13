namespace RecipeManagement.Domain.RecipeIngridients.Dtos;

using Destructurama.Attributed;

public sealed record RecipeIngredientDetailsDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }
    public decimal Count { get; set; }
    public string IngredientName { get; set; }
    public decimal Calories { get; set; }
    public string Unit { get; set; }
    public decimal Fat { get; set; }
    public decimal Carbs { get; set; }
    public decimal Protein { get; set; }
    public decimal Sugar { get; set; }
}
