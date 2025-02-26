namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.UserFavorites.Features;
using RecipeManagement.Domain.UserFavorites.Dtos;
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
[Route("api/v{v:apiVersion}/userfavorites")]
[ApiVersion("1.0")]
public sealed class UserFavoritesController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new UserFavorite record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddUserFavorite")]
    public async Task<ActionResult<UserFavoriteDto>> AddUserFavorite([FromBody]UserFavoriteForCreationDto userFavoriteForCreation)
    {
        var command = new AddUserFavorite.Command(userFavoriteForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetUserFavorite",
            new { userFavoriteId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a list of all UserFavorites.
    /// </summary>
    [HttpGet(Name = "GetUserFavorites")]
    public async Task<IActionResult> GetUserFavorites([FromQuery] UserFavoriteParametersDto userFavoriteParametersDto)
    {
        var query = new GetUserFavoriteList.Query(userFavoriteParametersDto);
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
    /// Deletes an existing UserFavorite record.
    /// </summary>
    [Authorize]
    [HttpDelete("{userFavoriteId:guid}", Name = "DeleteUserFavorite")]
    public async Task<ActionResult> DeleteUserFavorite(Guid userFavoriteId)
    {
        var command = new DeleteUserFavorite.Command(userFavoriteId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
