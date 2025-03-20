namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.RecipeIngridients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class RecipeIngridientConfiguration : IEntityTypeConfiguration<RecipeIngridient>
{
    /// <summary>
    /// The database configuration for RecipeIngridients. 
    /// </summary>
    public void Configure(EntityTypeBuilder<RecipeIngridient> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}