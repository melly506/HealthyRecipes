namespace RecipeManagement.Domain.Seasons.Dtos;

using Destructurama.Attributed;

public sealed record SeasonForCreationDto
{
    public string Name { get; set; }
}
