namespace RecipeManagement.Domain.Comments.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Domain.Comments.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddComment
{
    public sealed record Command(CommentForCreationDto CommentToAdd) : IRequest<CommentDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, CommentDto>
    {
        public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var commentToAdd = request.CommentToAdd.ToCommentForCreation();
            var comment = Comment.Create(commentToAdd);

            await dbContext.Comments.AddAsync(comment, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return comment.ToCommentDto();
        }
    }
}