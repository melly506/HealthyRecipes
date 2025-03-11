namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.RecipeIngridients.Features;
using RecipeManagement.Domain.RecipeIngridients.Dtos;
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
[Route("api/v{v:apiVersion}/recipeingridients")]
[ApiVersion("1.0")]
public sealed class RecipeIngridientsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Updates an entire existing RecipeIngridient.
    /// </summary>
    [Authorize]
    [HttpPut("{recipeIngridientId:guid}", Name = "UpdateRecipeIngridient")]
    public async Task<IActionResult> UpdateRecipeIngridient(Guid recipeIngridientId, RecipeIngridientForUpdateDto recipeIngridient)
    {
        var command = new UpdateRecipeIngridient.Command(recipeIngridientId, recipeIngridient);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing RecipeIngridient record.
    /// </summary>
    [Authorize]
    [HttpDelete("{recipeIngridientId:guid}", Name = "DeleteRecipeIngridient")]
    public async Task<ActionResult> DeleteRecipeIngridient(Guid recipeIngridientId)
    {
        var command = new DeleteRecipeIngridient.Command(recipeIngridientId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
