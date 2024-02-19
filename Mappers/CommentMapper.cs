
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment comment)
        {
            return new CommentDTO{
                Id = comment.Id,
                Title= comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn.ToLocalTime(),
                StockId = comment.StockId,
                CreatedBy = comment.AppUser.UserName
            };
        }

        public static Comment ToCommentFromCreateDTO(this CreateCommentDTO comment, int stockId)
        {
            return new Comment{
                Title= comment.Title,
                Content = comment.Content,
                StockId = stockId
            };
        }



    }
}

