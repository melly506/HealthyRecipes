namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Diets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class DietConfiguration : IEntityTypeConfiguration<Diet>
{
    /// <summary>
    /// The database configuration for Diets. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Diet> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}