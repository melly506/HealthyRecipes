namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.DishTypes.Features;
using RecipeManagement.Domain.DishTypes.Dtos;
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
[Route("api/v{v:apiVersion}/dishtypes")]
[ApiVersion("1.0")]
public sealed class DishTypesController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new DishType record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddDishType")]
    public async Task<ActionResult<DishTypeDto>> AddDishType([FromBody]DishTypeForCreationDto dishTypeForCreation)
    {
        var command = new AddDishType.Command(dishTypeForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetDishType",
            new { dishTypeId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single DishType by ID.
    /// </summary>
    [HttpGet("{dishTypeId:guid}", Name = "GetDishType")]
    public async Task<ActionResult<DishTypeDto>> GetDishType(Guid dishTypeId)
    {
        var query = new GetDishType.Query(dishTypeId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all DishTypes.
    /// </summary>
    [HttpGet(Name = "GetDishTypes")]
    public async Task<IActionResult> GetDishTypes([FromQuery] DishTypeParametersDto dishTypeParametersDto)
    {
        var query = new GetDishTypeList.Query(dishTypeParametersDto);
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
    /// Updates an entire existing DishType.
    /// </summary>
    [Authorize]
    [HttpPut("{dishTypeId:guid}", Name = "UpdateDishType")]
    public async Task<IActionResult> UpdateDishType(Guid dishTypeId, DishTypeForUpdateDto dishType)
    {
        var command = new UpdateDishType.Command(dishTypeId, dishType);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing DishType record.
    /// </summary>
    [Authorize]
    [HttpDelete("{dishTypeId:guid}", Name = "DeleteDishType")]
    public async Task<ActionResult> DeleteDishType(Guid dishTypeId)
    {
        var command = new DeleteDishType.Command(dishTypeId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
