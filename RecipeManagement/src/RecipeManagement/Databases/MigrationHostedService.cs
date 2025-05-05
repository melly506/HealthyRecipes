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
                Ingredient.Create(new IngredientForCreation { Name = "Індичка", Calories = 189, Unit = "g", Fat = 7.3m, Carbs = 0m, Protein = 29m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Айсберг", Calories = 14, Unit = "g", Fat = 0.1m, Carbs = 3.0m, Protein = 0.9m, Sugar = 1.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Арахісова паста", Calories = 588, Unit = "g", Fat = 50.0m, Carbs = 20.0m, Protein = 25.0m, Sugar = 9.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Артишоки", Calories = 47, Unit = "g", Fat = 0.2m, Carbs = 10.5m, Protein = 3.3m, Sugar = 1.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Базилік", Calories = 22, Unit = "g", Fat = 0.6m, Carbs = 2.7m, Protein = 3.2m, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Баклажан", Calories = 25, Unit = "g", Fat = 0.2m, Carbs = 6.0m, Protein = 1.0m, Sugar = 3.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Баранина", Calories = 294, Unit = "g", Fat = 21m, Carbs = 0m, Protein = 25m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Батат", Calories = 86, Unit = "g", Fat = 0.1m, Carbs = 20.1m, Protein = 1.6m, Sugar = 4.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Борошно пшеничне", Calories = 364, Unit = "g", Fat = 1, Carbs = 76, Protein = 10, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Брокколі", Calories = 34, Unit = "g", Fat = 0.4m, Carbs = 7.0m, Protein = 2.8m, Sugar = 1.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Буряк", Calories = 43, Unit = "g", Fat = 0.2m, Carbs = 9.6m, Protein = 1.6m, Sugar = 6.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Білий гриб", Calories = 34, Unit = "g", Fat = 0.4m, Carbs = 6.7m, Protein = 3.7m, Sugar = 0.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Вершковий сир", Calories = 342, Unit = "g", Fat = 34.0m, Carbs = 4.0m, Protein = 6.0m, Sugar = 3.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Виноград", Calories = 69, Unit = "g", Fat = 0.2m, Carbs = 18.0m, Protein = 0.7m, Sugar = 16.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Вода", Calories = 0, Unit = "ml", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Вівсяні пластівці", Calories = 389, Unit = "g", Fat = 6.9m, Carbs = 66, Protein = 16.9m, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Гарбуз", Calories = 26, Unit = "g", Fat = 0.1m, Carbs = 6.5m, Protein = 1.0m, Sugar = 2.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Горошок зелений", Calories = 81, Unit = "g", Fat = 0.4m, Carbs = 14.0m, Protein = 5.4m, Sugar = 5.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Грецькі горіхи", Calories = 654, Unit = "g", Fat = 65.0m, Carbs = 14.0m, Protein = 15.0m, Sugar = 2.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Гречка", Calories = 343, Unit = "g", Fat = 3.4m, Carbs = 71.5m, Protein = 13.3m, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Гриб Вешенка", Calories = 33, Unit = "g", Fat = 0.4m, Carbs = 6.1m, Protein = 3.3m, Sugar = 2.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "Гриб Вешенка", Calories = 35, Unit = "g", Fat = 0.3m, Carbs = 6.5m, Protein = 2.4m, Sugar = 3.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Гриб Лисичка", Calories = 38, Unit = "g", Fat = 0.5m, Carbs = 6.9m, Protein = 1.5m, Sugar = 2.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Гриби Шиітаке", Calories = 34, Unit = "g", Fat = 0.5m, Carbs = 7m, Protein = 2.2m, Sugar = 2.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Гриби Печериці", Calories = 22, Unit = "g", Fat = 0.3m, Carbs = 3.3m, Protein = 3.1m, Sugar = 2.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Гірчиця", Calories = 66, Unit = "g", Fat = 4.2m, Carbs = 5.8m, Protein = 4.4m, Sugar = 0.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Желатин (порошок)", Calories = 355, Unit = "g", Fat = 0m, Carbs = 0m, Protein = 85m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Йогурт грецький", Calories = 121, Unit = "g", Fat = 10m, Carbs = 3.6m, Protein = 8.5m, Sugar = 3.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Йогурт натуральний", Calories = 59, Unit = "g", Fat = 3.3m, Carbs = 3.6m, Protein = 3.5m, Sugar = 3.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Какао-порошок", Calories = 228, Unit = "g", Fat = 11.7m, Carbs = 57.9m, Protein = 19.6m, Sugar = 1.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Капуста білокачанна", Calories = 25, Unit = "g", Fat = 0.1m, Carbs = 5.8m, Protein = 1.3m, Sugar = 3.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Картопля", Calories = 77, Unit = "g", Fat = 0.1m, Carbs = 17, Protein = 2, Sugar = 0.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Квасоля біла (суха)", Calories = 333, Unit = "g", Fat = 1.1m, Carbs = 60.0m, Protein = 23.4m, Sugar = 2.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кетчуп", Calories = 101, Unit = "g", Fat = 0.3m, Carbs = 25, Protein = 1.3m, Sugar = 22 }),
                Ingredient.Create(new IngredientForCreation { Name = "Кленовий сироп", Calories = 260, Unit = "ml", Fat = 0m, Carbs = 67m, Protein = 0.1m, Sugar = 60m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кокосова стружка", Calories = 660, Unit = "g", Fat = 65.0m, Carbs = 24.2m, Protein = 6.9m, Sugar = 7.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кокосове молоко", Calories = 230, Unit = "ml", Fat = 24.0m, Carbs = 6.0m, Protein = 2.3m, Sugar = 3.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Коріандр", Calories = 23, Unit = "g", Fat = 0.5m, Carbs = 3.7m, Protein = 2.1m, Sugar = 0.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кролик", Calories = 173, Unit = "g", Fat = 8.2m, Carbs = 0m, Protein = 21.1m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кріп", Calories = 43, Unit = "g", Fat = 1.1m, Carbs = 7.0m, Protein = 3.5m, Sugar = 0.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кукурудза консервована", Calories = 86, Unit = "g", Fat = 1.35m, Carbs = 19.0m, Protein = 3.4m, Sugar = 6.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кукурудза", Calories = 86, Unit = "g", Fat = 1.35m, Carbs = 19.0m, Protein = 3.4m, Sugar = 6.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Кукурудзяний сироп", Calories = 286, Unit = "ml", Fat = 0m, Carbs = 79m, Protein = 0m, Sugar = 79m }),
                Ingredient.Create(new IngredientForCreation { Name = "Лляне насіння", Calories = 534, Unit = "g", Fat = 42.0m, Carbs = 29.0m, Protein = 18.0m, Sugar = 1.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Лосось", Calories = 208, Unit = "g", Fat = 13, Carbs = 0, Protein = 20, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Лісові горіхи", Calories = 628, Unit = "g", Fat = 61.0m, Carbs = 17.0m, Protein = 15.0m, Sugar = 4.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "М'ята", Calories = 70, Unit = "g", Fat = 0.9m, Carbs = 15.0m, Protein = 3.8m, Sugar = 0.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "Майонез", Calories = 680, Unit = "g", Fat = 75, Carbs = 1.8m, Protein = 1.1m, Sugar = 1.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Масло вершкове", Calories = 717, Unit = "g", Fat = 81, Carbs = 0.6m, Protein = 0.9m, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Мед", Calories = 304, Unit = "g", Fat = 0, Carbs = 82, Protein = 0.3m, Sugar = 82 }),
                Ingredient.Create(new IngredientForCreation { Name = "Мигдаль", Calories = 579, Unit = "g", Fat = 49.0m, Carbs = 22.0m, Protein = 21.0m, Sugar = 3.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Мигдальне молоко", Calories = 15, Unit = "ml", Fat = 1.2m, Carbs = 0.3m, Protein = 0.6m, Sugar = 0.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Молоко коров'яче", Calories = 42, Unit = "ml", Fat = 1, Carbs = 5, Protein = 3.4m, Sugar = 5 }),
                Ingredient.Create(new IngredientForCreation { Name = "Морква", Calories = 41, Unit = "g", Fat = 0.2m, Carbs = 10, Protein = 0.9m, Sugar = 4.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Моцарела", Calories = 280, Unit = "g", Fat = 17.0m, Carbs = 3.1m, Protein = 28.0m, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Насіння гарбуза", Calories = 559, Unit = "g", Fat = 49.0m, Carbs = 10.7m, Protein = 30.0m, Sugar = 1.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Насіння соняшника", Calories = 584, Unit = "g", Fat = 51.0m, Carbs = 20.0m, Protein = 21.0m, Sugar = 2.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Насіння чіа", Calories = 486, Unit = "g", Fat = 31.0m, Carbs = 42.0m, Protein = 17.0m, Sugar = 0.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Нут (сухий)", Calories = 364, Unit = "g", Fat = 6.0m, Carbs = 61.0m, Protein = 19.0m, Sugar = 10.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Огірки свіжі", Calories = 15, Unit = "g", Fat = 0.1m, Carbs = 3.6m, Protein = 0.7m, Sugar = 1.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія кукурудзяна", Calories = 884, Unit = "ml", Fat = 100m, Carbs = 0m, Protein = 0m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія кунжутна", Calories = 884, Unit = "ml", Fat = 100m, Carbs = 0m, Protein = 0m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія лляна", Calories = 884, Unit = "ml", Fat = 100m, Carbs = 0m, Protein = 0m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія оливкова", Calories = 880, Unit = "ml", Fat = 100, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія пальмова", Calories = 884, Unit = "ml", Fat = 100m, Carbs = 0m, Protein = 0m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія соняшникова", Calories = 880, Unit = "ml", Fat = 100, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Олія соєва", Calories = 884, Unit = "ml", Fat = 100m, Carbs = 0m, Protein = 0m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Орегано", Calories = 265, Unit = "g", Fat = 4.3m, Carbs = 68.0m, Protein = 9.0m, Sugar = 4.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Оцет бальзамічний", Calories = 88, Unit = "ml", Fat = 0.0m, Carbs = 17.0m, Protein = 0.5m, Sugar = 17.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Оцет яблучний", Calories = 21, Unit = "ml", Fat = 0.0m, Carbs = 0.9m, Protein = 0.0m, Sugar = 0.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Паприка сушена", Calories = 282, Unit = "g", Fat = 13, Carbs = 54, Protein = 14, Sugar = 10 }),
                Ingredient.Create(new IngredientForCreation { Name = "Пекінська капуста", Calories = 13, Unit = "g", Fat = 0.2m, Carbs = 2.2m, Protein = 1.5m, Sugar = 1.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Перець болгарський", Calories = 31, Unit = "g", Fat = 0.3m, Carbs = 6.0m, Protein = 1.0m, Sugar = 4.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Перець чорний мелений", Calories = 251, Unit = "g", Fat = 3.3m, Carbs = 64, Protein = 10, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Петрушка", Calories = 36, Unit = "g", Fat = 0.8m, Carbs = 6.3m, Protein = 3.0m, Sugar = 0.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Помідори", Calories = 18, Unit = "g", Fat = 0.2m, Carbs = 3.9m, Protein = 0.9m, Sugar = 2.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Редис", Calories = 16, Unit = "g", Fat = 0.1m, Carbs = 3.4m, Protein = 0.7m, Sugar = 1.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Рис білий", Calories = 365, Unit = "g", Fat = 0.7m, Carbs = 80, Protein = 7, Sugar = 0.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "Розмарин", Calories = 331, Unit = "g", Fat = 13.0m, Carbs = 64.0m, Protein = 3.3m, Sugar = 1.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Розпушувач тіста", Calories = 53, Unit = "g", Fat = 0.0m, Carbs = 28.0m, Protein = 0.0m, Sugar = 0.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Рукола", Calories = 25, Unit = "g", Fat = 0.7m, Carbs = 3.7m, Protein = 2.6m, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Салат латук", Calories = 15, Unit = "g", Fat = 0.2m, Carbs = 2.9m, Protein = 1.4m, Sugar = 0.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Свинина", Calories = 242, Unit = "g", Fat = 14, Carbs = 0, Protein = 27, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Селера коренева", Calories = 42, Unit = "g", Fat = 0.3m, Carbs = 9.2m, Protein = 1.5m, Sugar = 1.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Селера стеблова", Calories = 16, Unit = "g", Fat = 0.2m, Carbs = 3.0m, Protein = 0.7m, Sugar = 1.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Брі", Calories = 334, Unit = "g", Fat = 27.7m, Carbs = 0.5m, Protein = 20.8m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Гауда", Calories = 356, Unit = "g", Fat = 27.2m, Carbs = 2.2m, Protein = 24.9m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Дорблю", Calories = 353, Unit = "g", Fat = 28.7m, Carbs = 2.3m, Protein = 21.4m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Емменталь", Calories = 380, Unit = "g", Fat = 29m, Carbs = 2.2m, Protein = 27m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Камамбер", Calories = 300, Unit = "g", Fat = 24m, Carbs = 0.5m, Protein = 19.8m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Пармезан", Calories = 431, Unit = "g", Fat = 29m, Carbs = 4.1m, Protein = 38m, Sugar = 0.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Пекоріно", Calories = 387, Unit = "g", Fat = 28m, Carbs = 3.7m, Protein = 25m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Рікота", Calories = 174, Unit = "g", Fat = 13m, Carbs = 3m, Protein = 11m, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Фета", Calories = 264, Unit = "g", Fat = 21.3m, Carbs = 4.1m, Protein = 14.2m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир Чеддер", Calories = 403, Unit = "g", Fat = 33.1m, Carbs = 1.3m, Protein = 24.9m, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сир твердий", Calories = 402, Unit = "g", Fat = 33, Carbs = 1.3m, Protein = 25, Sugar = 0.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сироп агави", Calories = 310, Unit = "ml", Fat = 0m, Carbs = 76m, Protein = 0m, Sugar = 76m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сметана", Calories = 193, Unit = "ml", Fat = 20, Carbs = 3.4m, Protein = 2.7m, Sugar = 3.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сода харчова", Calories = 0, Unit = "g", Fat = 0.0m, Carbs = 0.0m, Protein = 0.0m, Sugar = 0.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Солодовий сироп", Calories = 260, Unit = "ml", Fat = 0m, Carbs = 65m, Protein = 0.6m, Sugar = 35m }),
                Ingredient.Create(new IngredientForCreation { Name = "Соєвий соус", Calories = 53, Unit = "ml", Fat = 5.0m, Carbs = 4.9m, Protein = 8.0m, Sugar = 0.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "Спаржа", Calories = 20, Unit = "g", Fat = 0.1m, Carbs = 3.9m, Protein = 2.2m, Sugar = 1.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сухі дріжджі", Calories = 325, Unit = "g", Fat = 4.3m, Carbs = 40.0m, Protein = 40.0m, Sugar = 5.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сушені томати", Calories = 258, Unit = "g", Fat = 2.6m, Carbs = 55.0m, Protein = 14.0m, Sugar = 37.0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Сіль кухонна", Calories = 0, Unit = "g", Fat = 0, Carbs = 0, Protein = 0, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Творог нежирний", Calories = 98, Unit = "g", Fat = 3.0m, Carbs = 3.4m, Protein = 11.0m, Sugar = 3.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Телятина", Calories = 172, Unit = "g", Fat = 8.4m, Carbs = 0m, Protein = 20.5m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Тим'ян", Calories = 101, Unit = "g", Fat = 1.7m, Carbs = 24.0m, Protein = 5.6m, Sugar = 1.7m }),
                Ingredient.Create(new IngredientForCreation { Name = "Тофу", Calories = 76, Unit = "g", Fat = 4.8m, Carbs = 1.9m, Protein = 8.0m, Sugar = 0.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Утка", Calories = 337, Unit = "g", Fat = 28m, Carbs = 0m, Protein = 19m, Sugar = 0m }),
                Ingredient.Create(new IngredientForCreation { Name = "Фенхель", Calories = 31, Unit = "g", Fat = 0.2m, Carbs = 7.3m, Protein = 1.2m, Sugar = 3.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Форель", Calories = 190, Unit = "g", Fat = 11, Carbs = 0, Protein = 22, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Філе куряче", Calories = 165, Unit = "g", Fat = 3.6m, Carbs = 0, Protein = 31, Sugar = 0 }),
                Ingredient.Create(new IngredientForCreation { Name = "Фініковий сироп", Calories = 310, Unit = "ml", Fat = 0m, Carbs = 75m, Protein = 1m, Sugar = 63m }),
                Ingredient.Create(new IngredientForCreation { Name = "Цвітна капуста", Calories = 25, Unit = "g", Fat = 0.3m, Carbs = 5.0m, Protein = 1.9m, Sugar = 1.9m }),
                Ingredient.Create(new IngredientForCreation { Name = "Цибуля зелена", Calories = 32, Unit = "g", Fat = 0.3m, Carbs = 7.3m, Protein = 1.8m, Sugar = 2.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Цибуля ріпчаста", Calories = 40, Unit = "g", Fat = 0.1m, Carbs = 9, Protein = 1.1m, Sugar = 4.2m }),
                Ingredient.Create(new IngredientForCreation { Name = "Цукор-пісок", Calories = 387, Unit = "g", Fat = 0, Carbs = 100, Protein = 0, Sugar = 100 }),
                Ingredient.Create(new IngredientForCreation { Name = "Цукіні", Calories = 17, Unit = "g", Fat = 0.3m, Carbs = 3.1m, Protein = 1.2m, Sugar = 2.5m }),
                Ingredient.Create(new IngredientForCreation { Name = "Часник", Calories = 149, Unit = "g", Fat = 0.5m, Carbs = 33, Protein = 6.4m, Sugar = 1 }),
                Ingredient.Create(new IngredientForCreation { Name = "Червона капуста", Calories = 31, Unit = "g", Fat = 0.2m, Carbs = 7.4m, Protein = 1.4m, Sugar = 3.8m }),
                Ingredient.Create(new IngredientForCreation { Name = "Чорна квасоля (суха)", Calories = 339, Unit = "g", Fat = 0.9m, Carbs = 61.0m, Protein = 21.0m, Sugar = 2.1m }),
                Ingredient.Create(new IngredientForCreation { Name = "Шоколадний сироп", Calories = 250, Unit = "ml", Fat = 1.5m, Carbs = 62m, Protein = 1.5m, Sugar = 61m }),
                Ingredient.Create(new IngredientForCreation { Name = "Шпинат", Calories = 23, Unit = "g", Fat = 0.4m, Carbs = 3.6m, Protein = 2.9m, Sugar = 0.4m }),
                Ingredient.Create(new IngredientForCreation { Name = "Яблука", Calories = 52, Unit = "g", Fat = 0.2m, Carbs = 14, Protein = 0.3m, Sugar = 10 }),
                Ingredient.Create(new IngredientForCreation { Name = "Ягоди журавлини", Calories = 46, Unit = "g", Fat = 0.1m, Carbs = 12.0m, Protein = 0.4m, Sugar = 4.3m }),
                Ingredient.Create(new IngredientForCreation { Name = "Яйце куряче", Calories = 70, Unit = "g", Fat = 5, Carbs = 0.6m, Protein = 6, Sugar = 0.6m }),
                Ingredient.Create(new IngredientForCreation { Name = "Яйце перепелине", Calories = 14, Unit = "g", Fat = 1.2m, Carbs = 0.04m, Protein = 1.2m, Sugar = 0.04m }),
                Ingredient.Create(new IngredientForCreation { Name = "Яловичина", Calories = 250, Unit = "g", Fat = 15, Carbs = 0, Protein = 26, Sugar = 0 }),
            };

            await context.AddRangeAsync(ingredients, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
