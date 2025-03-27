namespace RecipeManagement.Controllers.v1;

using System.Text.Json;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Domain.Comments.Features;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Domain.RecipeIngridients.Features;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.Extensions.Filters;

[ApiController]
[Route("api/v{v:apiVersion}/recipes")]
[ApiVersion("1.0")]
public sealed class RecipesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new Recipe record.
    /// </summary>
    [Authorize]
    [Transaction]
    [HttpPost(Name = "AddRecipe")]
    public async Task<ActionResult<RecipeDto>> AddRecipe([FromBody] RecipeForCreationDto recipeForCreation)
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

        // Attach food types to recipe
        var attachFoodTypesCommand = new AttachFoodTypesToRecipe.Command(recipe.Id, recipeForCreation.FoodTypeIds);
        await mediator.Send(attachFoodTypesCommand);

        // Attach diets to recipe
        var attachDietsCommand = new AttachDietsToRecipe.Command(recipe.Id, recipeForCreation.DietIds);
        await mediator.Send(attachDietsCommand);

        // Attach seasons to recipe
        var attachSeasonsCommand = new AttachSeasonsToRecipe.Command(recipe.Id, recipeForCreation.SeasonIds);
        await mediator.Send(attachSeasonsCommand);

        // Attach dish types to recipe
        var attachDishTypesCommand = new AttachDishTypesToRecipe.Command(recipe.Id, recipeForCreation.DishTypeIds);
        await mediator.Send(attachDishTypesCommand);

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
    [Transaction]
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

        // Create comand to remove all previous food types from recipe
        var deleteFoodTypesCommand = new DeleteFoodTypeFromRecipe.Command(recipeId);
        // Attach food types to recipe
        var attachFoodTypesCommand = new AttachFoodTypesToRecipe.Command(recipeId, recipeUpdate.FoodTypeIds);

        // Create comand to remove all previous diets from recipe
        var deleteDietsCommand = new DeleteDietsFromRecipe.Command(recipeId);
        // Attach diets to recipe
        var attachDietsCommand = new AttachDietsToRecipe.Command(recipeId, recipeUpdate.DietIds);

        // Create comand to remove all previous seasons from recipe
        var deleteSeasonsCommand = new DeleteSeasonsFromRecipe.Command(recipeId);
        // Attach seasons to recipe
        var attachSeasonsCommand = new AttachSeasonsToRecipe.Command(recipeId, recipeUpdate.SeasonIds);

        // Create comand to remove all previous dishTypes from recipe
        var deleteDishTypesCommand = new DeleteDishTypesFromRecipe.Command(recipeId);
        // Attach dishTypes to recipe
        var attachDishTypesCommand = new AttachDishTypesToRecipe.Command(recipeId, recipeUpdate.DishTypeIds);

        await mediator.Send(updateRecipeCommand);
        // Send ingredients update
        await mediator.Send(deleteIngredientsCommand);
        await mediator.Send(updateIngredientsCommand);

        // Send food types update
        await mediator.Send(deleteFoodTypesCommand);
        await mediator.Send(attachFoodTypesCommand);

        // Send diets update
        await mediator.Send(deleteDietsCommand);
        await mediator.Send(attachDietsCommand);

        // Send seasons update
        await mediator.Send(deleteSeasonsCommand);
        await mediator.Send(attachSeasonsCommand);

        // Send dish types update
        await mediator.Send(deleteDishTypesCommand);
        await mediator.Send(attachDishTypesCommand);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Recipe record.
    /// </summary>
    [Authorize]
    [Transaction]
    [HttpDelete("{recipeId:guid}", Name = "DeleteRecipe")]
    public async Task<ActionResult> DeleteRecipe(Guid recipeId)
    {

        // Create comand to remove all recipe ingridients attached to recipe and send this
        var deleteIngredientsCommand = new DeleteRecipeIngridientsByRecipeId.Command(recipeId);
        await mediator.Send(deleteIngredientsCommand);

        // Create comand to remove all recipe food types attached to recipe and send this
        var deleteFoodTypesCommand = new DeleteFoodTypeFromRecipe.Command(recipeId);
        await mediator.Send(deleteFoodTypesCommand);

        // Create comand to remove all recipe diets attached to recipe and send this
        var deleteDietsCommand = new DeleteDietsFromRecipe.Command(recipeId);
        await mediator.Send(deleteDietsCommand);

        // Create comand to remove all recipe seasons attached to recipe and send this
        var deleteSeasonsCommand = new DeleteSeasonsFromRecipe.Command(recipeId);
        await mediator.Send(deleteSeasonsCommand);

        // Create comand to remove all recipe dishTypes attached to recipe and send this
        var deleteDishTypesCommand = new DeleteDishTypesFromRecipe.Command(recipeId);
        await mediator.Send(deleteDishTypesCommand);

        // Remove recipe
        var deleteRecipeCommand = new DeleteRecipe.Command(recipeId);
        await mediator.Send(deleteRecipeCommand);
        return NoContent();
    }

    /// <summary>
    /// Creates a comment in Recipe
    /// </summary>
    [Authorize]
    [HttpPost("{recipeId:guid}/addComment", Name = "AddCommentToRecipe")]
    public async Task<ActionResult<CommentDto>> AddCommentToRecipe(Guid recipeId, CommentForCreationDto commentForCreation)
    {
        var addCommentToRecipeCommand = new AddCommentToRecipe.Command(recipeId, commentForCreation);
        var commandResponse = await mediator.Send(addCommentToRecipeCommand);

        return CreatedAtRoute("GetComment",
            new { commentId = commandResponse.Id },
            commandResponse);
    }

    /// <summary>
    /// Gets a list of all Recipe Comments.
    /// </summary>
    [HttpGet("{recipeId:guid}/comments", Name = "GetRecipeComments")]
    public async Task<IActionResult> GetRecipeComments(Guid recipeId, [FromQuery] CommentParametersDto commentParametersDto)
    {
        var query = new GetCommentList.Query(commentParametersDto, recipeId);
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

}
