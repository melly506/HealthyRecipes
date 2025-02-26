namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.RecipeIngridients.Features;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
using RecipeManagement.Resources;
using RecipeManagement.Domain;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using Asp.Versioning;
using MediatR;

[ApiController]
[Route("api/v{v:apiVersion}/recipeingridients")]
[ApiVersion("1.0")]
public sealed class RecipeIngridientsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new RecipeIngridient record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddRecipeIngridient")]
    public async Task<ActionResult<RecipeIngridientDto>> AddRecipeIngridient([FromBody]RecipeIngridientForCreationDto recipeIngridientForCreation)
    {
        var command = new AddRecipeIngridient.Command(recipeIngridientForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetRecipeIngridient",
            new { recipeIngridientId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a list of all RecipeIngridients.
    /// </summary>
    [HttpGet(Name = "GetRecipeIngridients")]
    public async Task<IActionResult> GetRecipeIngridients([FromQuery] RecipeIngridientParametersDto recipeIngridientParametersDto)
    {
        var query = new GetRecipeIngridientList.Query(recipeIngridientParametersDto);
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
    /// Updates an entire existing RecipeIngridient.
    /// </summary>
    [Authorize]
    [HttpPut("{recipeIngridientId:guid}", Name = "UpdateRecipeIngridient")]
    public async Task<IActionResult> UpdateRecipeIngridient(Guid recipeIngridientId, RecipeIngridientForUpdateDto recipeIngridient)
    {
        var command = new UpdateRecipeIngridient.Command(recipeIngridientId, recipeIngridient);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing RecipeIngridient record.
    /// </summary>
    [Authorize]
    [HttpDelete("{recipeIngridientId:guid}", Name = "DeleteRecipeIngridient")]
    public async Task<ActionResult> DeleteRecipeIngridient(Guid recipeIngridientId)
    {
        var command = new DeleteRecipeIngridient.Command(recipeIngridientId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
