namespace RecipeManagement.Domain.Diets.Dtos;

using Destructurama.Attributed;

public sealed record DietForCreationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
