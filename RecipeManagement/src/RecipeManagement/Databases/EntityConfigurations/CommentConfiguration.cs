namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    /// <summary>
    /// The database configuration for Comments. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Relationship Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        builder.HasOne(x => x.User)
            .WithMany(x => x.Comments);

        // Property Marker -- Deleting or modifying this comment could cause incomplete relationship scaffolding
        
    }
}
