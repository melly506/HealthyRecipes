namespace RecipeManagement.Domain.FoodTypes.Dtos;

using Destructurama.Attributed;

public sealed record FoodTypeForCreationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
