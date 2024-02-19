

using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public sealed class CommentRepository : ICommentRepository
    {
        #region FIELDS
        private readonly ApplicationDbContext context;

        #endregion

        #region CTOR
        public CommentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion


        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await context.Comments.AddAsync(commentModel);
            int result = await context.SaveChangesAsync();

            if (result != 1)
                throw new InvalidOperationException("Could not save data");
            else
                return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            Comment? stockModel = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel is null)
                return null;

            else
                context.Comments.Remove(stockModel);

            int result = await context.SaveChangesAsync();

            if (result != 1)
                throw new InvalidOperationException("Could not delete data");
            return stockModel;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(Helpers.CommentQueryObject queryObject)
        {
            var comments = context.Comments
                        .AsNoTracking()
                        .Include(c => c.AppUser)
                        .AsQueryable();

            if(!String.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol);
            }
            if(queryObject.IsDesc == true)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            else
                comments = comments.OrderBy(c => c.CreatedOn);


            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await context.Comments
                    .AsNoTracking()
                    .Include(c => c.AppUser)
                    .FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null)
                return null;
            else
                return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDTO commentUpdate)
        {
            Comment? commentModel = await context.Comments
                .FirstOrDefaultAsync(x => x.Id == id);

            if (commentModel is null)
                return null;

            #region UPDATE
            commentModel.Title = commentUpdate.Title;
            commentModel.Content = commentUpdate.Content;
            #endregion

            int result = await context.SaveChangesAsync();

            if (result != 1)
                throw new InvalidOperationException("Could not update data");
            else 
                return commentModel;
        }
    }
}