namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;
using RecipeManagement.Domain.FoodTypes.Dtos;

public sealed record RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int CookingTime { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int LikesCount { get; set; }
    public bool IsDraft { get; set; }
    public List<FoodTypeDto> FoodType { get; set; } = new();

    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }
}
