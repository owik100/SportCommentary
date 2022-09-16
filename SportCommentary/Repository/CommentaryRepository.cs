using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository
{
    public class CommentaryRepository : ICommentaryRepository
    {
        private readonly ApplicationDbContext _dataContext;
        public CommentaryRepository(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Commentary>> GetAllCommentaryAsync()
        {
            return await _dataContext.Commentary.ToListAsync();
        }

        public async Task<ICollection<Commentary>> GetAllCommentaryLiveAsync()
        {
            return await _dataContext.Commentary.Where(comm => comm.IsLive).ToListAsync();
        }

        public async Task<bool> CommentaryExistAsync(string CommentaryName)
        {
            return await _dataContext.Commentary.AnyAsync(Comm => Comm.Caption == CommentaryName);
        }

        public async Task<Commentary> GetCommentaryByIdAsync(int commentaryId)
        {
            return await _dataContext.Commentary.FirstOrDefaultAsync(comm => comm.CommentaryID == commentaryId);
        }

        public async Task<bool> CreateCommentaryAsync(Commentary commentary)
        {
            await _dataContext.Commentary.AddAsync(commentary);
            return await Save();
        }

        public async Task<bool> UpdateCommentaryAsync(Commentary commentary)
        {
            _dataContext.Commentary.Update(commentary);
            return await Save();
        }

        public async Task<bool> DeleteCommentaryAsync(Commentary commentary)
        {
            _dataContext.Remove(commentary);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
