namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.FoodTypes.Features;
using RecipeManagement.Domain.FoodTypes.Dtos;
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
[Route("api/v{v:apiVersion}/foodtypes")]
[ApiVersion("1.0")]
public sealed class FoodTypesController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new FoodType record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddFoodType")]
    public async Task<ActionResult<FoodTypeDto>> AddFoodType([FromBody]FoodTypeForCreationDto foodTypeForCreation)
    {
        var command = new AddFoodType.Command(foodTypeForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetFoodType",
            new { foodTypeId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single FoodType by ID.
    /// </summary>
    [HttpGet("{foodTypeId:guid}", Name = "GetFoodType")]
    public async Task<ActionResult<FoodTypeDto>> GetFoodType(Guid foodTypeId)
    {
        var query = new GetFoodType.Query(foodTypeId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all FoodTypes.
    /// </summary>
    [HttpGet(Name = "GetFoodTypes")]
    public async Task<IActionResult> GetFoodTypes([FromQuery] FoodTypeParametersDto foodTypeParametersDto)
    {
        var query = new GetFoodTypeList.Query(foodTypeParametersDto);
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
    /// Updates an entire existing FoodType.
    /// </summary>
    [Authorize]
    [HttpPut("{foodTypeId:guid}", Name = "UpdateFoodType")]
    public async Task<IActionResult> UpdateFoodType(Guid foodTypeId, FoodTypeForUpdateDto foodType)
    {
        var command = new UpdateFoodType.Command(foodTypeId, foodType);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing FoodType record.
    /// </summary>
    [Authorize]
    [HttpDelete("{foodTypeId:guid}", Name = "DeleteFoodType")]
    public async Task<ActionResult> DeleteFoodType(Guid foodTypeId)
    {
        var command = new DeleteFoodType.Command(foodTypeId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
