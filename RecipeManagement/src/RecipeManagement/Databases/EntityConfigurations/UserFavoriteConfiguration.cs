namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.UserFavorites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
{
    /// <summary>
    /// The database configuration for UserFavorites. 
    /// </summary>
    public void Configure(EntityTypeBuilder<UserFavorite> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        builder.HasOne(x => x.User)
            .WithMany(x => x.UserFavorites);

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}
