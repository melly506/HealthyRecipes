namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Users.Features;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Domain.Comments.Features;
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
[Route("api/v{v:apiVersion}/users")]
[ApiVersion("1.0")]
public sealed class UsersController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a list of all Users.
    /// </summary>
    [Authorize]
    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery] UserParametersDto userParametersDto)
    {
        var query = new GetUserList.Query(userParametersDto);
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
    /// Gets a single User by ID.
    /// </summary>
    [Authorize]
    [HttpGet("{userId:guid}", Name = "GetUser")]
    public async Task<ActionResult<UserDto>> GetUser(Guid userId)
    {
        var query = new GetUser.Query(userId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }

    /// <summary>
    /// Gets the current authenticated user's profile.
    /// Creates the user in the database from keycloak token data if they don't exist.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var query = new GetOrCreateCurrentUser.Query();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Updates the current authenticated user's profile.
    /// </summary>
    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult> UpdateCurrentUser(UserForUpdateDto user)
    {
        var command = new UpdateCurrentUser.Command(user);
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Gets a list of all User Comments.
    /// </summary>
    [Authorize]
    [HttpGet("me/comments", Name = "GetCurrentUserComments")]
    public async Task<IActionResult> GetCurrentUserComments([FromQuery] CommentParametersDto commentParametersDto)
    {
        var query = new GetCommentList.Query(commentParametersDto);
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

    // endpoint marker - do not delete this comment
}
