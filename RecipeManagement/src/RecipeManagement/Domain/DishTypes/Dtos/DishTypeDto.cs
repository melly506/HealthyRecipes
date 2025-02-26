namespace RecipeManagement.Domain.DishTypes.Dtos;

using Destructurama.Attributed;

public sealed record DishTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
