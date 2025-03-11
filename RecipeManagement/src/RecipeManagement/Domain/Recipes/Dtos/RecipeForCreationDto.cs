namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;

public sealed record RecipeIngridientAssignDto
{
    public decimal Count { get; set; }
    public Guid IngridientId { get; set; }
}

public sealed record RecipeForCreationDto
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int CookingTime { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public List<RecipeIngridientAssignDto> RecipeIngridientsAssign { get; set; } = new();
}
