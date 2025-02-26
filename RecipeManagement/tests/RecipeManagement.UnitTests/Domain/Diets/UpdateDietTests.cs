namespace RecipeManagement.UnitTests.Domain.Diets;

using RecipeManagement.SharedTestHelpers.Fakes.Diet;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateDietTests
{
    private readonly Faker _faker;

    public UpdateDietTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_diet()
    {
        // Arrange
        var diet = new FakeDietBuilder().Build();
        var updatedDiet = new FakeDietForUpdate().Generate();
        
        // Act
        diet.Update(updatedDiet);

        // Assert
        diet.Name.Should().Be(updatedDiet.Name);
        diet.Description.Should().Be(updatedDiet.Description);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var diet = new FakeDietBuilder().Build();
        var updatedDiet = new FakeDietForUpdate().Generate();
        diet.DomainEvents.Clear();
        
        // Act
        diet.Update(updatedDiet);

        // Assert
        diet.DomainEvents.Count.Should().Be(1);
        diet.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(DietUpdated));
    }
}