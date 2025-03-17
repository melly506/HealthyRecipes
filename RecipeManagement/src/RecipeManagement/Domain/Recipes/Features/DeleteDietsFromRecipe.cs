namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;


public static class DeleteDietsFromRecipe
{
    public sealed record Command(Guid RecipeId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
    : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            string sqlCommand = @"DELETE FROM dbo.diet_recipe 
                                 WHERE recipes_id = @recipeId";

            await dbContext.Database.ExecuteSqlRawAsync(
                sqlCommand,
                new Microsoft.Data.SqlClient.SqlParameter("@recipeId", request.RecipeId)
            );
        }
    }
}
