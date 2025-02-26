namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.RolePermissions.Features;
using RecipeManagement.Domain.RolePermissions.Dtos;
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
[Route("api/v{v:apiVersion}/rolepermissions")]
[ApiVersion("1.0")]
public sealed class RolePermissionsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a list of all RolePermissions.
    /// </summary>
    [Authorize]
    [HttpGet(Name = "GetRolePermissions")]
    public async Task<IActionResult> GetRolePermissions([FromQuery] RolePermissionParametersDto rolePermissionParametersDto)
    {
        var query = new GetRolePermissionList.Query(rolePermissionParametersDto);
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
    /// Gets a single RolePermission by ID.
    /// </summary>
    [Authorize]
    [HttpGet("{rolePermissionId:guid}", Name = "GetRolePermission")]
    public async Task<ActionResult<RolePermissionDto>> GetRolePermission(Guid rolePermissionId)
    {
        var query = new GetRolePermission.Query(rolePermissionId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new RolePermission record.
    /// </summary>
    [Authorize]
    [HttpPost(Name = "AddRolePermission")]
    public async Task<ActionResult<RolePermissionDto>> AddRolePermission([FromBody]RolePermissionForCreationDto rolePermissionForCreation)
    {
        var command = new AddRolePermission.Command(rolePermissionForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetRolePermission",
            new { rolePermissionId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing RolePermission.
    /// </summary>
    [Authorize]
    [HttpPut("{rolePermissionId:guid}", Name = "UpdateRolePermission")]
    public async Task<IActionResult> UpdateRolePermission(Guid rolePermissionId, RolePermissionForUpdateDto rolePermission)
    {
        var command = new UpdateRolePermission.Command(rolePermissionId, rolePermission);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing RolePermission record.
    /// </summary>
    [Authorize]
    [HttpDelete("{rolePermissionId:guid}", Name = "DeleteRolePermission")]
    public async Task<ActionResult> DeleteRolePermission(Guid rolePermissionId)
    {
        var command = new DeleteRolePermission.Command(rolePermissionId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
