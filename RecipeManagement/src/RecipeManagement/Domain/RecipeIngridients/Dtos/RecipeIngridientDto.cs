namespace RecipeManagement.Domain.RecipeIngridients.Dtos;

using Destructurama.Attributed;

public sealed record RecipeIngridientDto
{
    public Guid Id { get; set; }
    public decimal Count { get; set; }
}
