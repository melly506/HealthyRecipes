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
        
        // example for a more complex value object
        // builder.OwnsOne(x => x.PhysicalAddress, opts =>
        // {
        //     opts.Property(x => x.Line1).HasColumnName("physical_address_line1");
        //     opts.Property(x => x.Line2).HasColumnName("physical_address_line2");
        //     opts.Property(x => x.City).HasColumnName("physical_address_city");
        //     opts.Property(x => x.State).HasColumnName("physical_address_state");
        //     opts.OwnsOne(x => x.PostalCode, o =>
        //         {
        //             o.Property(x => x.Value).HasColumnName("physical_address_postal_code");
        //         }).Navigation(x => x.PostalCode)
        //         .IsRequired();
        //     opts.Property(x => x.Country).HasColumnName("physical_address_country");
        // }).Navigation(x => x.PhysicalAddress)
        //     .IsRequired();
    }
}
