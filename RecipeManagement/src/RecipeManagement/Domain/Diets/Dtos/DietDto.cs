namespace RecipeManagement.Domain.Diets.Dtos;

using Destructurama.Attributed;

public sealed record DietDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
