namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Comments.Features;
using RecipeManagement.Domain.Comments.Dtos;
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
[Route("api/v{v:apiVersion}/comments")]
[ApiVersion("1.0")]
public sealed class CommentsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a single Comment by ID.
    /// </summary>
    [HttpGet("{commentId:guid}", Name = "GetComment")]
    public async Task<ActionResult<CommentDto>> GetComment(Guid commentId)
    {
        var query = new GetComment.Query(commentId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Updates an entire existing Comment.
    /// </summary>
    [Authorize]
    [HttpPut("{commentId:guid}", Name = "UpdateComment")]
    public async Task<IActionResult> UpdateComment(Guid commentId, CommentForUpdateDto comment)
    {
        var command = new UpdateComment.Command(commentId, comment);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Comment record.
    /// </summary>
    [Authorize]
    [HttpDelete("{commentId:guid}", Name = "DeleteComment")]
    public async Task<ActionResult> DeleteComment(Guid commentId)
    {
        var command = new DeleteComment.Command(commentId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
