namespace RecipeManagement.Domain.Seasons.Models;

using Destructurama.Attributed;

public sealed record SeasonForCreation
{
    public string Name { get; set; }
}
