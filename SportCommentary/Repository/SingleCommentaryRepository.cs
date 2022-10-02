using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Pages;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SingleCommentary;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository
{
    public class SingleCommentaryRepository : ISingleCommentaryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dataContext;
        public SingleCommentaryRepository(IDbContextFactory<ApplicationDbContext> dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreateSingleCommentaryAsync(SingleComment singleComment)
        {
            using var context = _dataContext.CreateDbContext();

            await context.SingleComment.AddAsync(singleComment);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> DeleteSingleCommentAsync(SingleComment singleComment)
        {
            using var context = _dataContext.CreateDbContext();

            context.Remove(singleComment);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<ICollection<SingleComment>> GetByCommentaryIdAsync(int Id)
        {
            using var context = _dataContext.CreateDbContext();

            return await context.SingleComment
                .Include(sinnCom => sinnCom.Event)
                .Where(sinComm => sinComm.CommentaryID == Id)
                .OrderByDescending(sinnComm => sinnComm.Time)
                .ToListAsync();
        }

        public async Task<SingleComment> GetByIdAsync(int Id)
        {
            using var context = _dataContext.CreateDbContext();

            return await context.SingleComment.FirstOrDefaultAsync(sinComm => sinComm.SingleCommentID == Id);
        }
    }
}
