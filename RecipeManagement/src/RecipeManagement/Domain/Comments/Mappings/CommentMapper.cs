namespace RecipeManagement.Domain.Comments.Mappings;

using RecipeManagement.Domain.Comments.Dtos;
using RecipeManagement.Domain.Comments.Models;
using RecipeManagement.Domain.Users.Mappings;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class CommentMapper
{
    public static partial CommentForCreation ToCommentForCreation(this CommentForCreationDto commentForCreationDto);
    public static partial CommentForUpdate ToCommentForUpdate(this CommentForUpdateDto commentForUpdateDto);
    public static partial IQueryable<CommentDto> ToCommentDtoQueryable(this IQueryable<Comment> comment);

  
    public static partial CommentDto ToCommentDtoBase(this Comment comment);

  
    public static CommentDto ToCommentDto(this Comment comment)
    {
        var dto = comment.ToCommentDtoBase();

        dto.User = comment.User?.ToUserDto();

        return dto;
    }
}