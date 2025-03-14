namespace RecipeManagement.Controllers.v1;

using System.Text.Json;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Domain.RecipeIngridients.Features;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Features;

[ApiController]
[Route("api/v{v:apiVersion}/recipes")]
[ApiVersion("1.0")]
public sealed class RecipesController(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Creates a new Recipe record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddRecipe")]
    public async Task<ActionResult<RecipeDto>> AddRecipe([FromBody]RecipeForCreationDto recipeForCreation)
    {

        var addRecipeCommand = new AddRecipe.Command(recipeForCreation);
        var recipe = await mediator.Send(addRecipeCommand);

        // Convert all recipe ingredients to be added
        var ingredientsToAdd = recipeForCreation.RecipeIngridientsAssign
            .Select(i => new RecipeIngridientForCreationDto
            {
                Count = i.Count,
                IngredientId = i.IngridientId,
                RecipeId = recipe.Id
            })
            .ToList();

        var addIngredientsCommand = new AddRecipeIngridients.Command(ingredientsToAdd);
        var recipeIngredients = await mediator.Send(addIngredientsCommand);


        return CreatedAtRoute(
            "GetRecipe",
            new { recipeId = recipe.Id },
            new { recipe, recipeIngredients }
        );
    }


    /// <summary>
    /// Gets a single Recipe by ID.
    /// </summary>
    [HttpGet("{recipeId:guid}", Name = "GetRecipe")]
    public async Task<ActionResult<RecipeDto>> GetRecipe(Guid recipeId)
    {
        var getRecipeCommand = new GetRecipe.Query(recipeId);
        var recipe = await mediator.Send(getRecipeCommand);

        var getIngridientDetailsCommand = new GetRecipeIngredientDetailsList.Query(recipeId);
        var ingridientsDetails = await mediator.Send(getIngridientDetailsCommand);
        return Ok(new { recipe, ingridientsDetails });
    }


    /// <summary>
    /// Gets a list of all Recipes.
    /// </summary>
    [HttpGet(Name = "GetRecipes")]
    public async Task<IActionResult> GetRecipes([FromQuery] RecipeParametersDto recipeParametersDto)
    {
        var query = new GetRecipeList.Query(recipeParametersDto);
        var queryResponse = await mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Updates an entire existing Recipe.
    /// </summary>
    [Authorize]
    [HttpPut("{recipeId:guid}", Name = "UpdateRecipe")]
    public async Task<IActionResult> UpdateRecipe(Guid recipeId, RecipeForUpdateDto recipeUpdate)
    {
        var updateRecipeCommand = new UpdateRecipe.Command(recipeId, recipeUpdate);

        // Prepare new recipe ingridients
        var ingredientsForUpdate = recipeUpdate.RecipeIngridientsAssign
            .Select(i => new RecipeIngridientForCreationDto
            {
                Count = i.Count,
                IngredientId = i.IngridientId,
                RecipeId = recipeId
            })
            .ToList();


        // Create comand to remove all previous recipe ingridients from recipe
        var deleteIngredientsCommand = new DeleteRecipeIngridientsByRecipeId.Command(recipeId);

        // Create comand to assign new recipe ingridients to recipe
        var updateIngredientsCommand = new AddRecipeIngridients.Command(ingredientsForUpdate);

        await mediator.Send(updateRecipeCommand);
        await mediator.Send(deleteIngredientsCommand);
        await mediator.Send(updateIngredientsCommand);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Recipe record.
    /// </summary>
    [Authorize]
    [HttpDelete("{recipeId:guid}", Name = "DeleteRecipe")]
    public async Task<ActionResult> DeleteRecipe(Guid recipeId)
    {

        var deleteIngredientsCommand = new DeleteRecipeIngridientsByRecipeId.Command(recipeId);
        await mediator.Send(deleteIngredientsCommand);

        var deleteRecipeCommand = new DeleteRecipe.Command(recipeId);
        await mediator.Send(deleteRecipeCommand);
        return NoContent();
    }
}
