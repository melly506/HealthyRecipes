namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.FoodTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class FoodTypeConfiguration : IEntityTypeConfiguration<FoodType>
{
    /// <summary>
    /// The database configuration for FoodTypes. 
    /// </summary>
    public void Configure(EntityTypeBuilder<FoodType> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}