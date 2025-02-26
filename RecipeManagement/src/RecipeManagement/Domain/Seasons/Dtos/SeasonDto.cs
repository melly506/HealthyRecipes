namespace RecipeManagement.Domain.Seasons.Dtos;

using Destructurama.Attributed;

public sealed record SeasonDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
