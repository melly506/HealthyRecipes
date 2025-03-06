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
    Diet.Create(new DietForCreation { Name = "Дієта #5" }),
    Diet.Create(new DietForCreation { Name = "Безглютенова дієта" }),
    Diet.Create(new DietForCreation { Name = "Діабетична дієта" }),
    Diet.Create(new DietForCreation { Name = "Високобілкова" })
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
    FoodType.Create(new FoodTypeForCreation { Name = "Вегетаріанство" }),
    FoodType.Create(new FoodTypeForCreation { Name = "Веганство" }),
    FoodType.Create(new FoodTypeForCreation { Name = "Пескетаріанці" }),
    FoodType.Create(new FoodTypeForCreation { Name = "Омніворія" })
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
    Season.Create(new SeasonForCreation { Name = "Зима" }),
    Season.Create(new SeasonForCreation { Name = "Весна" }),
    Season.Create(new SeasonForCreation { Name = "Літо" }),
    Season.Create(new SeasonForCreation { Name = "Осінь" })
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
    DishType.Create(new DishTypeForCreation { Name = "Закуски" }),
    DishType.Create(new DishTypeForCreation { Name = "Перші страви" }),
    DishType.Create(new DishTypeForCreation { Name = "Другі страви" }),
    DishType.Create(new DishTypeForCreation { Name = "Десерти" }),
    DishType.Create(new DishTypeForCreation { Name = "Гарячі" }),
    DishType.Create(new DishTypeForCreation { Name = "Холодні" })
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
    Ingredient.Create(new IngredientForCreation { Name = "Вода", Calories = 0, Unit = "ml", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Сіль кухонна", Calories = 0, Unit = "g", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Цукор-пісок", Calories = 387, Unit = "g", Fat = 0, Carbs = 100, Protein = 0, Sugar = 100 }),
    Ingredient.Create(new IngredientForCreation { Name = "Яйце куряче", Calories = 70, Unit = "count", Fat = 5, Carbs = 0.6m, Protein = 6, Sugar = 0.6m }),
    Ingredient.Create(new IngredientForCreation { Name = "Молоко коров'яче", Calories = 42, Unit = "ml", Fat = 1, Carbs = 5, Protein = 3.4m, Sugar = 5 }),
    Ingredient.Create(new IngredientForCreation { Name = "Борошно пшеничне", Calories = 364, Unit = "g", Fat = 1, Carbs = 76, Protein = 10, Sugar = 0.3m }),
    Ingredient.Create(new IngredientForCreation { Name = "Олія соняшникова", Calories = 880, Unit = "ml", Fat = 100, Carbs = 0, Protein = 0, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Олія оливкова", Calories = 880, Unit = "ml", Fat = 100, Carbs = 0, Protein = 0, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Цибуля ріпчаста", Calories = 40, Unit = "g", Fat = 0.1m, Carbs = 9, Protein = 1.1m, Sugar = 4.2m }),
    Ingredient.Create(new IngredientForCreation { Name = "Часник", Calories = 149, Unit = "g", Fat = 0.5m, Carbs = 33, Protein = 6.4m, Sugar = 1 }),
    Ingredient.Create(new IngredientForCreation { Name = "Морква", Calories = 41, Unit = "g", Fat = 0.2m, Carbs = 10, Protein = 0.9m, Sugar = 4.7m }),
    Ingredient.Create(new IngredientForCreation { Name = "Картопля", Calories = 77, Unit = "g", Fat = 0.1m, Carbs = 17, Protein = 2, Sugar = 0.8m }),
    Ingredient.Create(new IngredientForCreation { Name = "Помідори", Calories = 18, Unit = "g", Fat = 0.2m, Carbs = 3.9m, Protein = 0.9m, Sugar = 2.6m }),
    Ingredient.Create(new IngredientForCreation { Name = "Огірки свіжі", Calories = 15, Unit = "g", Fat = 0.1m, Carbs = 3.6m, Protein = 0.7m, Sugar = 1.7m }),
    Ingredient.Create(new IngredientForCreation { Name = "Перець чорний мелений", Calories = 251, Unit = "g", Fat = 3.3m, Carbs = 64, Protein = 10, Sugar = 0.6m }),
    Ingredient.Create(new IngredientForCreation { Name = "Паприка сушена", Calories = 282, Unit = "g", Fat = 13, Carbs = 54, Protein = 14, Sugar = 10 }),
    Ingredient.Create(new IngredientForCreation { Name = "Філе куряче", Calories = 165, Unit = "g", Fat = 3.6m, Carbs = 0, Protein = 31, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Свинина", Calories = 242, Unit = "g", Fat = 14, Carbs = 0, Protein = 27, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Яловичина", Calories = 250, Unit = "g", Fat = 15, Carbs = 0, Protein = 26, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Лосось", Calories = 208, Unit = "g", Fat = 13, Carbs = 0, Protein = 20, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Форель", Calories = 190, Unit = "g", Fat = 11, Carbs = 0, Protein = 22, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Сметана", Calories = 193, Unit = "g", Fat = 20, Carbs = 3.4m, Protein = 2.7m, Sugar = 3.4m }),
    Ingredient.Create(new IngredientForCreation { Name = "Масло вершкове", Calories = 717, Unit = "g", Fat = 81, Carbs = 0.6m, Protein = 0.9m, Sugar = 0.6m }),
    Ingredient.Create(new IngredientForCreation { Name = "Сир твердий", Calories = 402, Unit = "g", Fat = 33, Carbs = 1.3m, Protein = 25, Sugar = 0.5m }),
    Ingredient.Create(new IngredientForCreation { Name = "Йогурт натуральний", Calories = 59, Unit = "g", Fat = 3.3m, Carbs = 3.6m, Protein = 3.5m, Sugar = 3.6m }),
    Ingredient.Create(new IngredientForCreation { Name = "Рис білий", Calories = 365, Unit = "g", Fat = 0.7m, Carbs = 80, Protein = 7, Sugar = 0.1m }),
    Ingredient.Create(new IngredientForCreation { Name = "Гречка", Calories = 343, Unit = "g", Fat = 3.4m, Carbs = 71.5m, Protein = 13.3m, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Вівсяні пластівці", Calories = 389, Unit = "g", Fat = 6.9m, Carbs = 66, Protein = 16.9m, Sugar = 0 }),
    Ingredient.Create(new IngredientForCreation { Name = "Мед", Calories = 304, Unit = "g", Fat = 0, Carbs = 82, Protein = 0.3m, Sugar = 82 }),
    Ingredient.Create(new IngredientForCreation { Name = "Майонез", Calories = 680, Unit = "g", Fat = 75, Carbs = 1.8m, Protein = 1.1m, Sugar = 1.8m }),
    Ingredient.Create(new IngredientForCreation { Name = "Кетчуп", Calories = 101, Unit = "g", Fat = 0.3m, Carbs = 25, Protein = 1.3m, Sugar = 22 }),
    Ingredient.Create(new IngredientForCreation { Name = "Яблука", Calories = 52, Unit = "g", Fat = 0.2m, Carbs = 14, Protein = 0.3m, Sugar = 10 })
};

            await context.AddRangeAsync(ingredients, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
