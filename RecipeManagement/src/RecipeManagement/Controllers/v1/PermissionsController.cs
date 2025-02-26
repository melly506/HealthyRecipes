namespace RecipeManagement.Controllers.v1;

using Asp.Versioning;
using Domain;
using HeimGuard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Exceptions;

[ApiController]
[Route("api/v{v:apiVersion}/permissions")]
[ApiVersion("1.0")]
public sealed class PermissionsController(IHeimGuardClient heimGuard, IUserPolicyHandler userPolicyHandler) : ControllerBase
{
    /// <summary>
    /// Gets a list of all available permissions.
    /// </summary>
    [Authorize]
    [HttpGet(Name = "GetPermissions")]
    public List<string> GetPermissions()
    {
        heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanGetPermissions);
        return Permissions.List();
    }

    /// <summary>
    /// Gets a list of the current user's assigned permissions.
    /// </summary>
    [Authorize]
    [HttpGet("mine", Name = "GetAssignedPermissions")]
    public async Task<List<string>> GetAssignedPermissions()
    {
        var permissions = await userPolicyHandler.GetUserPermissions();
        return permissions.ToList();
    }
}