namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Ingredients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    /// <summary>
    /// The database configuration for Ingredients. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        builder.HasMany(x => x.RecipeIngridients)
            .WithOne(x => x.Ingredient);

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}
