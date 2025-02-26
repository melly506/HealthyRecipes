namespace RecipeManagement.Domain.RecipeIngridients.Dtos;

using Destructurama.Attributed;

public sealed record RecipeIngridientForUpdateDto
{
    public decimal Count { get; set; }
}
