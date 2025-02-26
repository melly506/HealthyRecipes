namespace RecipeManagement.Domain.Seasons.Models;

using Destructurama.Attributed;

public sealed record SeasonForUpdate
{
    public string Name { get; set; }
}
