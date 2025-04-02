namespace RecipeManagement.Extensions.Services;

using HeimGuard;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Microsoft.EntityFrameworkCore;

public static class HeimGuardExtensions
{
    public static async Task MustHaveRecipeOwnership(this IHeimGuardClient heimGuard,
        Guid recipeId,
        RecipesDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        var userId = currentUserService.UserId;

        var recipe = await dbContext.Recipes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == recipeId);

        if (recipe == null || recipe.CreatedBy != userId)
        {
            throw new NoRolesAssignedException();
        }
    }
}