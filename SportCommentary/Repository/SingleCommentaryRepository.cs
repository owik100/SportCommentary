using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SingleCommentary;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository
{
    public class SingleCommentaryRepository : ISingleCommentaryRepository
    {
        private readonly ApplicationDbContext _dataContext;
        public SingleCommentaryRepository(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreateSingleCommentaryAsync(SingleComment singleComment)
        {
            await _dataContext.SingleComment.AddAsync(singleComment);
            return await Save();
        }

        public async Task<bool> DeleteSingleCommentAsync(SingleComment singleComment)
        {
            _dataContext.Remove(singleComment);
            return await Save();
        }

        public async Task<ICollection<SingleComment>> GetByCommentaryIdAsync(int Id)
        {
           return await _dataContext.SingleComment
                .Include(sinnCom => sinnCom.Event)
                .Where(sinComm => sinComm.CommentaryID == Id)
                .ToListAsync();
        }

        public async Task<SingleComment> GetByIdAsync(int Id)
        {
            return await _dataContext.SingleComment.FirstOrDefaultAsync(sinComm => sinComm.SingleCommentID == Id);
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
