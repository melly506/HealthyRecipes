namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Models;
using RecipeManagement.Domain.Comments;
using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Domain.Comments.Models;
using RecipeManagement.Domain.Comments.Mappings;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;


public static class AddCommentToRecipe
{
    public sealed record Command(Guid RecipeId, CommentForCreationDto CommentToAdd) : IRequest<CommentDto>;

    public sealed class Handler(RecipesDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<Command, CommentDto>
    {
        public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Identifier == currentUserService.UserId, cancellationToken);
            var recipe = await dbContext.Recipes
              .GetById(request.RecipeId, cancellationToken);

            var commentToAdd = request.CommentToAdd.ToCommentForCreation();
            var comment = Comment.Create(commentToAdd);

            comment.SetUser(user);
            recipe.AddComment(comment);

            await dbContext.Comments.AddAsync(comment, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return comment.ToCommentDto();
        }
    }
}
