namespace RecipeManagement.Domain.FoodTypes.Dtos;

using Destructurama.Attributed;

public sealed record FoodTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
