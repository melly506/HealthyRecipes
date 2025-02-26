namespace RecipeManagement.Domain.Seasons.Dtos;

using Destructurama.Attributed;

public sealed record SeasonForUpdateDto
{
    public string Name { get; set; }
}
