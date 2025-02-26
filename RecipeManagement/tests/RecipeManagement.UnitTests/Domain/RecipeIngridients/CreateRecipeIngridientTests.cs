namespace RecipeManagement.UnitTests.Domain.RecipeIngridients;

using RecipeManagement.SharedTestHelpers.Fakes.RecipeIngridient;
using RecipeManagement.Domain.RecipeIngridients;
using RecipeManagement.Domain.RecipeIngridients.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateRecipeIngridientTests
{
    private readonly Faker _faker;

    public CreateRecipeIngridientTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_recipeIngridient()
    {
        // Arrange
        var recipeIngridientToCreate = new FakeRecipeIngridientForCreation().Generate();
        
        // Act
        var recipeIngridient = RecipeIngridient.Create(recipeIngridientToCreate);

        // Assert
        recipeIngridient.Count.Should().Be(recipeIngridientToCreate.Count);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var recipeIngridientToCreate = new FakeRecipeIngridientForCreation().Generate();
        
        // Act
        var recipeIngridient = RecipeIngridient.Create(recipeIngridientToCreate);

        // Assert
        recipeIngridient.DomainEvents.Count.Should().Be(1);
        recipeIngridient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeIngridientCreated));
    }
}