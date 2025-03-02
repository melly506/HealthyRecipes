namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Users.Features;
using RecipeManagement.Domain.Users.Dtos;
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
    /// Adds a new role to a user.
    /// </summary>
    [Authorize]
    [HttpPut("{userId:guid}/addRole", Name = "AddRole")]
    public async Task<IActionResult> AddRole([FromRoute] Guid userId, [FromBody] string role)
    {
        var command = new AddUserRole.Command(userId, role);
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Removes a role from a User
    /// </summary>
    [Authorize]
    [HttpPut("{userId:guid}/removeRole", Name = "RemoveRole")]
    public async Task<ActionResult> RemoveRole([FromRoute] Guid userId, [FromBody] string role)
    {
        var command = new RemoveUserRole.Command(userId, role);
        await mediator.Send(command);
        return NoContent();
    }


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
    /// Creates a new User record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddUser")]
    public async Task<ActionResult<UserDto>> AddUser([FromBody]UserForCreationDto userForCreation)
    {
        var command = new AddUser.Command(userForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetUser",
            new { userId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing User.
    /// </summary>
    [Authorize]
    [HttpPut("{userId:guid}", Name = "UpdateUser")]
    public async Task<IActionResult> UpdateUser(Guid userId, UserForUpdateDto user)
    {
        var command = new UpdateUser.Command(userId, user);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing User record.
    /// </summary>
    [Authorize]
    [HttpDelete("{userId:guid}", Name = "DeleteUser")]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        var command = new DeleteUser.Command(userId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
