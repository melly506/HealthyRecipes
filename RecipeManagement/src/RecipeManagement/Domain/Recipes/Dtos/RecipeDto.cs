namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;
using RecipeManagement.Domain.FoodTypes.Dtos;
using RecipeManagement.Domain.Diets.Dtos;
using RecipeManagement.Domain.Seasons.Dtos;
using RecipeManagement.Domain.DishTypes.Dtos;

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
    public List<DietDto> Diet { get; set; } = new();
    public List<SeasonDto> Season { get; set; } = new();
    public List<DishTypeDto> DishType { get; set; } = new();
    public bool IsLiked { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }
}
