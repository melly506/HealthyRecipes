namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Diets.Features;
using RecipeManagement.Domain.Diets.Dtos;
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
[Route("api/v{v:apiVersion}/diets")]
[ApiVersion("1.0")]
public sealed class DietsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new Diet record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddDiet")]
    public async Task<ActionResult<DietDto>> AddDiet([FromBody]DietForCreationDto dietForCreation)
    {
        var command = new AddDiet.Command(dietForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetDiet",
            new { dietId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Diet by ID.
    /// </summary>
    [HttpGet("{dietId:guid}", Name = "GetDiet")]
    public async Task<ActionResult<DietDto>> GetDiet(Guid dietId)
    {
        var query = new GetDiet.Query(dietId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Diets.
    /// </summary>
    [HttpGet(Name = "GetDiets")]
    public async Task<IActionResult> GetDiets([FromQuery] DietParametersDto dietParametersDto)
    {
        var query = new GetDietList.Query(dietParametersDto);
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
    /// Updates an entire existing Diet.
    /// </summary>
    [Authorize]
    [HttpPut("{dietId:guid}", Name = "UpdateDiet")]
    public async Task<IActionResult> UpdateDiet(Guid dietId, DietForUpdateDto diet)
    {
        var command = new UpdateDiet.Command(dietId, diet);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Diet record.
    /// </summary>
    [Authorize]
    [HttpDelete("{dietId:guid}", Name = "DeleteDiet")]
    public async Task<ActionResult> DeleteDiet(Guid dietId)
    {
        var command = new DeleteDiet.Command(dietId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
