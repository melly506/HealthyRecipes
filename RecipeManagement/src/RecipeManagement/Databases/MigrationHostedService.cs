namespace RecipeManagement.Databases;

using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeManagement.Domain.Diets;
using RecipeManagement.Domain.Diets.Models;
using RecipeManagement.Domain.DishTypes;
using RecipeManagement.Domain.DishTypes.Models;
using RecipeManagement.Domain.FoodTypes;
using RecipeManagement.Domain.FoodTypes.Models;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Domain.Seasons;
using RecipeManagement.Domain.Seasons.Models;
using Serilog;

public class MigrationHostedService<TDbContext>(
    IServiceScopeFactory scopeFactory)
    : IHostedService
    where TDbContext : DbContext
{
    private readonly ILogger _logger = Log.ForContext<MigrationHostedService<TDbContext>>();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("Applying migrations for {DbContext}", typeof(TDbContext).Name);

            await using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await context.Database.MigrateAsync(cancellationToken);

            _logger.Information("Migrations complete for {DbContext}", typeof(TDbContext).Name);

            _logger.Information("Add seeds for {DbContext}", typeof(TDbContext).Name);
            await SeedDataAsync(context, cancellationToken);
            _logger.Information("Adding seeds complete for {DbContext}", typeof(TDbContext).Name);
        }
        catch (Exception ex) when (ex is SocketException)
        {
            _logger.Error(ex, "Could not connect to the database. Please check the connection string and make sure the database is running.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while applying the database migrations.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task SeedDataAsync(TDbContext context, CancellationToken cancellationToken)
    {
        await _SeedDietsAsync(context, cancellationToken);
        await _SeedFoodTypeAsync(context, cancellationToken);
        await _SeedSeasonAsync(context, cancellationToken);
        await _SeedDishTypeAsync(context, cancellationToken);
        await _SeedIngridients(context, cancellationToken);
    }

    private async Task _SeedDietsAsync(TDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Set<Diet>().AnyAsync(cancellationToken))
        {
            var diets = new List<Diet>
            {
                Diet.Create(new DietForCreation { Name = "ĳ��� #5" }),
                Diet.Create(new DietForCreation { Name = "������������ 䳺��" }),
                Diet.Create(new DietForCreation { Name = "ĳ�������� 䳺��" }),
                Diet.Create(new DietForCreation { Name = "������������" })
            };
            await context.AddRangeAsync(diets, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task _SeedFoodTypeAsync(TDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Set<FoodType>().AnyAsync(cancellationToken))
        {
            var foodTypes = new List<FoodType>
            {
                FoodType.Create(new FoodTypeForCreation { Name = "�������������" }),
                FoodType.Create(new FoodTypeForCreation { Name = "���������" }),
                FoodType.Create(new FoodTypeForCreation { Name = "������������" }),
                FoodType.Create(new FoodTypeForCreation { Name = "�������" })
            };
            await context.AddRangeAsync(foodTypes, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task _SeedSeasonAsync(TDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Set<Season>().AnyAsync(cancellationToken))
        {
            var seasons = new List<Season>
            {
                Season.Create(new SeasonForCreation { Name = "����" }),
                Season.Create(new SeasonForCreation { Name = "�����" }),
                Season.Create(new SeasonForCreation { Name = "˳��" }),
                Season.Create(new SeasonForCreation { Name = "����" })
            };
            await context.AddRangeAsync(seasons, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task _SeedDishTypeAsync(TDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Set<DishType>().AnyAsync(cancellationToken))
        {
            var dishTypes = new List<DishType>
            {
                DishType.Create(new DishTypeForCreation { Name = "�������" }),
                DishType.Create(new DishTypeForCreation { Name = "����� ������" }),
                DishType.Create(new DishTypeForCreation { Name = "���� ������" }),
                DishType.Create(new DishTypeForCreation { Name = "�������" }),
                DishType.Create(new DishTypeForCreation { Name = "������" }),
                DishType.Create(new DishTypeForCreation { Name = "������" })
            };
            await context.AddRangeAsync(dishTypes, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task _SeedIngridients(TDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Set<Ingredient>().AnyAsync(cancellationToken))
        {
            var ingredients = new List<Ingredient>
            {
                Ingredient.Create(new IngredientForCreation { Name = "����", Calories = 0, Unit = "ml", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "ѳ�� �������", Calories = 0, Unit = "g", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "�����-����", Calories = 387, Unit = "g", Fat = 0, Carbs = 100, Protein = 0, Sugar = 100 }),
                Ingredient.Create(new IngredientForCreation { Name = "���� ������", Calories = 70, Unit = "count", Fat = 5, Carbs = 0.6m, Protein = 6, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "������ �����'���", Calories = 42, Unit = "ml", Fat = 1, Carbs = 5, Protein = 3.4m, Sugar = 5 }),
                Ingredient.Create(new IngredientForCreation { Name = "������� ��������", Calories = 364, Unit = "g", Fat = 1, Carbs = 76, Protein = 10, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "��� �����������", Calories = 880, Unit = "ml", Fat = 100, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "��� ��������", Calories = 880, Unit = "ml", Fat = 100, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "������ �������", Calories = 40, Unit = "g", Fat = 0.1m, Carbs = 9, Protein = 1.1m, Sugar = 4.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 149, Unit = "g", Fat = 0.5m, Carbs = 33, Protein = 6.4m, Sugar = 1 }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 41, Unit = "g", Fat = 0.2m, Carbs = 10, Protein = 0.9m, Sugar = 4.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "��������", Calories = 77, Unit = "g", Fat = 0.1m, Carbs = 17, Protein = 2, Sugar = 0.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "�������", Calories = 18, Unit = "g", Fat = 0.2m, Carbs = 3.9m, Protein = 0.9m, Sugar = 2.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "����� ���", Calories = 15, Unit = "g", Fat = 0.1m, Carbs = 3.6m, Protein = 0.7m, Sugar = 1.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "������ ������ �������", Calories = 251, Unit = "g", Fat = 3.3m, Carbs = 64, Protein = 10, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "������� ������", Calories = 282, Unit = "g", Fat = 13, Carbs = 54, Protein = 14, Sugar = 10 }),
                Ingredient.Create(new IngredientForCreation { Name = "Գ�� ������", Calories = 165, Unit = "g", Fat = 3.6m, Carbs = 0, Protein = 31, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "�������", Calories = 242, Unit = "g", Fat = 14, Carbs = 0, Protein = 27, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "���������", Calories = 250, Unit = "g", Fat = 15, Carbs = 0, Protein = 26, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 208, Unit = "g", Fat = 13, Carbs = 0, Protein = 20, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 190, Unit = "g", Fat = 11, Carbs = 0, Protein = 22, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "�������", Calories = 193, Unit = "g", Fat = 20, Carbs = 3.4m, Protein = 2.7m, Sugar = 3.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "����� ��������", Calories = 717, Unit = "g", Fat = 81, Carbs = 0.6m, Protein = 0.9m, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "��� �������", Calories = 402, Unit = "g", Fat = 33, Carbs = 1.3m, Protein = 25, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "������ �����������", Calories = 59, Unit = "g", Fat = 3.3m, Carbs = 3.6m, Protein = 3.5m, Sugar = 3.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "��� ����", Calories = 365, Unit = "g", Fat = 0.7m, Carbs = 80, Protein = 7, Sugar = 0.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 343, Unit = "g", Fat = 3.4m, Carbs = 71.5m, Protein = 13.3m, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "³���� ��������", Calories = 389, Unit = "g", Fat = 6.9m, Carbs = 66, Protein = 16.9m, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "���", Calories = 304, Unit = "g", Fat = 0, Carbs = 82, Protein = 0.3m, Sugar = 82 }),
                Ingredient.Create(new IngredientForCreation { Name = "�������", Calories = 680, Unit = "g", Fat = 75, Carbs = 1.8m, Protein = 1.1m, Sugar = 1.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 101, Unit = "g", Fat = 0.3m, Carbs = 25, Protein = 1.3m, Sugar = 22 }),
                Ingredient.Create(new IngredientForCreation { Name = "���� �����", Calories = 53, Unit = "ml", Fat = 0.1m, Carbs = 8, Protein = 8, Sugar = 2 }),
                Ingredient.Create(new IngredientForCreation { Name = "���� ������", Calories = 0, Unit = "g", Fat = 0, Carbs = 0, Protein = 0.1m, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "��� ������", Calories = 0, Unit = "g", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "�����-�������", Calories = 228, Unit = "g", Fat = 14, Carbs = 58, Protein = 20, Sugar = 1.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "���� ����", Calories = 325, Unit = "g", Fat = 7, Carbs = 41, Protein = 40, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "�������", Calories = 335, Unit = "g", Fat = 0.1m, Carbs = 0, Protein = 85, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 573, Unit = "g", Fat = 50, Carbs = 23, Protein = 18, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "����� �������", Calories = 654, Unit = "g", Fat = 65, Carbs = 14, Protein = 15, Sugar = 2.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "�������", Calories = 579, Unit = "g", Fat = 50, Carbs = 22, Protein = 21, Sugar = 4.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "��������", Calories = 299, Unit = "g", Fat = 0.5m, Carbs = 79, Protein = 3.1m, Sugar = 59 }),
                Ingredient.Create(new IngredientForCreation { Name = "Գ���", Calories = 277, Unit = "g", Fat = 0.2m, Carbs = 75, Protein = 1.8m, Sugar = 66 }),
                Ingredient.Create(new IngredientForCreation { Name = "׳� ������", Calories = 486, Unit = "g", Fat = 31, Carbs = 42, Protein = 17, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "�������� ������", Calories = 230, Unit = "ml", Fat = 24, Carbs = 6, Protein = 2.3m, Sugar = 3.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "������� �����", Calories = 82, Unit = "g", Fat = 0.5m, Carbs = 18, Protein = 4.3m, Sugar = 12 }),
                Ingredient.Create(new IngredientForCreation { Name = "������� �������", Calories = 5, Unit = "ml", Fat = 0.2m, Carbs = 0.5m, Protein = 0.7m, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "��� ����", Calories = 264, Unit = "g", Fat = 21, Carbs = 4, Protein = 14, Sugar = 4 }),
                Ingredient.Create(new IngredientForCreation { Name = "������� ������", Calories = 546, Unit = "g", Fat = 31, Carbs = 61, Protein = 4.9m, Sugar = 48 }),
                Ingredient.Create(new IngredientForCreation { Name = "����� ��������", Calories = 387, Unit = "g", Fat = 0, Carbs = 100, Protein = 0, Sugar = 100 }),
                Ingredient.Create(new IngredientForCreation { Name = "������", Calories = 52, Unit = "g", Fat = 0.2m, Carbs = 14, Protein = 0.3m, Sugar = 10 })
            };

            await context.AddRangeAsync(ingredients, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
