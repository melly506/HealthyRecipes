namespace RecipeManagement.UnitTests.Domain.Diets;

using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateDietTests
{
    private readonly Faker _faker;

    public CreateDietTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_diet()
    {
        // Arrange
        var dietToCreate = new FakeDietForCreation().Generate();
        
        // Act
        var diet = Diet.Create(dietToCreate);

        // Assert
        diet.Name.Should().Be(dietToCreate.Name);
        diet.Description.Should().Be(dietToCreate.Description);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var dietToCreate = new FakeDietForCreation().Generate();
        
        // Act
        var diet = Diet.Create(dietToCreate);

        // Assert
        diet.DomainEvents.Count.Should().Be(1);
        diet.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(DietCreated));
    }
}