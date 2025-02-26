namespace RecipeManagement.Domain.RecipeIngridients.Dtos;

using Destructurama.Attributed;

public sealed record RecipeIngridientForCreationDto
{
    public decimal Count { get; set; }
}
