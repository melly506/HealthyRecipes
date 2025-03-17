namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Seasons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    /// <summary>
    /// The database configuration for Seasons. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}