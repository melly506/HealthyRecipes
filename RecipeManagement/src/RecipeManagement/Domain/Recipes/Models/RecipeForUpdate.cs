namespace RecipeManagement.Domain.Recipes.Models;

using Destructurama.Attributed;

public sealed record RecipeForUpdate
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int CookingTime { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public List<Guid> FoodTypeIds { get; set; } = new();
    public List<Guid> DietIds { get; set; } = new();
    public List<Guid> SeasonIds { get; set; } = new();
    public List<Guid> DishTypeIds { get; set; } = new();
}
