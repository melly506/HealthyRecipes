namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.DishTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class DishTypeConfiguration : IEntityTypeConfiguration<DishType>
{
    /// <summary>
    /// The database configuration for DishTypes. 
    /// </summary>
    public void Configure(EntityTypeBuilder<DishType> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}