namespace RecipeManagement.Controllers.v1;

using System.Text.Json;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Features;
using RecipeManagement.Domain.RecipeIngridients.Features;
using RecipeManagement.Extensions.Filters;

[ApiController]
[Route("api/v{v:apiVersion}/ingredients")]
[ApiVersion("1.0")]
public sealed class IngredientsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new Ingredient record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddIngredient")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IngredientForCreationDto ingredientForCreation)
    {
        var command = new AddIngredient.Command(ingredientForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetIngredient",
            new { ingredientId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Ingredient by ID.
    /// </summary>
    [HttpGet("{ingredientId:guid}", Name = "GetIngredient")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(Guid ingredientId)
    {
        var query = new GetIngredient.Query(ingredientId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Ingredients.
    /// </summary>
    [HttpGet(Name = "GetIngredients")]
    public async Task<IActionResult> GetIngredients([FromQuery] IngredientParametersDto ingredientParametersDto)
    {
        var query = new GetIngredientList.Query(ingredientParametersDto);
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
    /// Updates an entire existing Ingredient.
    /// </summary>
    [Authorize]
    [HttpPut("{ingredientId:guid}", Name = "UpdateIngredient")]
    public async Task<IActionResult> UpdateIngredient(Guid ingredientId, IngredientForUpdateDto ingredient)
    {
        var command = new UpdateIngredient.Command(ingredientId, ingredient);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Ingredient record.
    /// </summary>
    [Authorize]
    [Transaction]
    [HttpDelete("{ingredientId:guid}", Name = "DeleteIngredient")]
    public async Task<ActionResult> DeleteIngredient(Guid ingredientId)
    {
        var deleteIngredientsCommand = new DeleteRecipeIngridientsByIngridientId.Command(ingredientId);
        await mediator.Send(deleteIngredientsCommand);

        var deleteIngridientCommand = new DeleteIngredient.Command(ingredientId);
        await mediator.Send(deleteIngridientCommand);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
