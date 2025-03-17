namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    /// <summary>
    /// The database configuration for Recipes. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        builder.HasMany(x => x.FoodType)
            .WithMany(x => x.Recipes);
        builder.HasMany(x => x.Diet)
            .WithMany(x => x.Recipes);
        builder.HasMany(x => x.Season)
            .WithMany(x => x.Recipes);
        builder.HasMany(x => x.DishType)
            .WithMany(x => x.Recipes);
        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Recipe);
        builder.HasMany(x => x.UserFavorites)
            .WithOne(x => x.Recipe);
        builder.HasMany(x => x.RecipeIngridients)
            .WithOne(x => x.Recipe);

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}
