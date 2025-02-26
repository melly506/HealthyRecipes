namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Seasons.Features;
using RecipeManagement.Domain.Seasons.Dtos;
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
[Route("api/v{v:apiVersion}/seasons")]
[ApiVersion("1.0")]
public sealed class SeasonsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new Season record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddSeason")]
    public async Task<ActionResult<SeasonDto>> AddSeason([FromBody]SeasonForCreationDto seasonForCreation)
    {
        var command = new AddSeason.Command(seasonForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetSeason",
            new { seasonId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Season by ID.
    /// </summary>
    [HttpGet("{seasonId:guid}", Name = "GetSeason")]
    public async Task<ActionResult<SeasonDto>> GetSeason(Guid seasonId)
    {
        var query = new GetSeason.Query(seasonId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Seasons.
    /// </summary>
    [HttpGet(Name = "GetSeasons")]
    public async Task<IActionResult> GetSeasons([FromQuery] SeasonParametersDto seasonParametersDto)
    {
        var query = new GetSeasonList.Query(seasonParametersDto);
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
    /// Updates an entire existing Season.
    /// </summary>
    [Authorize]
    [HttpPut("{seasonId:guid}", Name = "UpdateSeason")]
    public async Task<IActionResult> UpdateSeason(Guid seasonId, SeasonForUpdateDto season)
    {
        var command = new UpdateSeason.Command(seasonId, season);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Season record.
    /// </summary>
    [Authorize]
    [HttpDelete("{seasonId:guid}", Name = "DeleteSeason")]
    public async Task<ActionResult> DeleteSeason(Guid seasonId)
    {
        var command = new DeleteSeason.Command(seasonId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
