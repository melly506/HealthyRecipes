namespace RecipeManagement.Domain.Ingredients.Dtos;

using Destructurama.Attributed;

public sealed record IngredientDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Calories { get; set; }
    public string Unit { get; set; }
    public decimal Fat { get; set; }
    public decimal Carbs { get; set; }
    public decimal Protein { get; set; }
    public decimal Sugar { get; set; }

}
