namespace RecipeManagement.Controllers.v1;

using Asp.Versioning;
using Domain;
using Domain.Roles;
using HeimGuard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Exceptions;

[ApiController]
[Route("api/v{v:apiVersion}/roles")]
[ApiVersion("1.0")]
public sealed class RolesController(IHeimGuardClient heimGuard): ControllerBase
{
    /// <summary>
    /// Gets a list of all available roles.
    /// </summary>
    [Authorize]
    [HttpGet(Name = "GetRoles")]
    public List<string> GetRoles()
    {
        heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanGetRoles);
        return Role.ListNames();
    }
}