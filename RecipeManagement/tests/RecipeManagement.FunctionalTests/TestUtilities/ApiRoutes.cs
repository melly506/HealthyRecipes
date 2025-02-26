namespace RecipeManagement.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

    public static class FoodTypes
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/foodTypes";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/foodTypes/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/foodTypes/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/foodTypes/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/foodTypes/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/foodTypes";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/foodTypes/batch";
    }

    public static class Diets
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/diets";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/diets/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/diets/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/diets/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/diets/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/diets";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/diets/batch";
    }

    public static class Seasons
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/seasons";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/seasons/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/seasons/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/seasons/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/seasons/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/seasons";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/seasons/batch";
    }

    public static class DishTypes
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/dishTypes";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/dishTypes/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/dishTypes/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/dishTypes/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/dishTypes/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/dishTypes";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/dishTypes/batch";
    }

    public static class Ingredients
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/ingredients";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/ingredients/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/ingredients/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/ingredients/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/ingredients/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/ingredients";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/ingredients/batch";
    }

    public static class UserFavorites
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/userFavorites";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/userFavorites/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/userFavorites/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/userFavorites/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/userFavorites/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/userFavorites";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/userFavorites/batch";
    }

    public static class Comments
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/comments";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/comments/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/comments/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/comments/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/comments/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/comments";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/comments/batch";
    }

    public static class RecipeIngridients
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/recipeIngridients";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/recipeIngridients/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/recipeIngridients/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/recipeIngridients/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/recipeIngridients/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/recipeIngridients";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/recipeIngridients/batch";
    }

    public static class Recipes
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/recipes";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/recipes/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/recipes/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/recipes/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/recipes/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/recipes";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/recipes/batch";
    }

    public static class Users
    {
        public static string GetList(string version = "v1")  => $"{Base}/{version}/users";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}";
        public static string Create(string version = "v1")  => $"{Base}/{version}/users";
        public static string CreateBatch(string version = "v1")  => $"{Base}/{version}/users/batch";
        public static string AddRole(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}/addRole";
        public static string RemoveRole(Guid id, string version = "v1") => $"{Base}/{version}/users/{id}/removeRole";
    }

    public static class RolePermissions
    {
        public static string GetList(string version = "v1") => $"{Base}/{version}/rolePermissions";
        public static string GetAll(string version = "v1") => $"{Base}/{version}/rolePermissions/all";
        public static string GetRecord(Guid id, string version = "v1") => $"{Base}/{version}/rolePermissions/{id}";
        public static string Delete(Guid id, string version = "v1") => $"{Base}/{version}/rolePermissions/{id}";
        public static string Put(Guid id, string version = "v1") => $"{Base}/{version}/rolePermissions/{id}";
        public static string Create(string version = "v1") => $"{Base}/{version}/rolePermissions";
        public static string CreateBatch(string version = "v1") => $"{Base}/{version}/rolePermissions/batch";
    }
}
