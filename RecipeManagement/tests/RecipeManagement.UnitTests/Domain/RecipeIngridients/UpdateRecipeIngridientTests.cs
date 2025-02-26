namespace RecipeManagement.UnitTests.Domain.RecipeIngridients;

using RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateRecipeIngridientTests
{
    private readonly Faker _faker;

    public UpdateRecipeIngridientTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_recipeIngridient()
    {
        // Arrange
        var recipeIngridient = new FakeRecipeIngridientBuilder().Build();
        var updatedRecipeIngridient = new FakeRecipeIngridientForUpdate().Generate();
        
        // Act
        recipeIngridient.Update(updatedRecipeIngridient);

        // Assert
        recipeIngridient.Count.Should().Be(updatedRecipeIngridient.Count);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var recipeIngridient = new FakeRecipeIngridientBuilder().Build();
        var updatedRecipeIngridient = new FakeRecipeIngridientForUpdate().Generate();
        recipeIngridient.DomainEvents.Clear();
        
        // Act
        recipeIngridient.Update(updatedRecipeIngridient);

        // Assert
        recipeIngridient.DomainEvents.Count.Should().Be(1);
        recipeIngridient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeIngridientUpdated));
    }
}