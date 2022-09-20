using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess.Entities;
using System.Linq;

namespace SportCommentary.Repository
{
    public class CommentaryRepository : ICommentaryRepository
    {
        private readonly ApplicationDbContext _dataContext;
        public CommentaryRepository(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Commentary>> GetAllCommentaryAsync(List<int> requestedCommentary)
        {
            if(requestedCommentary is null)
            {
                return await _dataContext.Commentary.OrderByDescending(x => x.CommentaryStart).ToListAsync();
            }
            return await _dataContext.Commentary.Where(comm=> requestedCommentary.Contains(comm.CommentaryID)).OrderByDescending(x => x.CommentaryStart).ToListAsync();
        }

        public async Task<ICollection<Commentary>> GetAllCommentaryLiveAsync(List<int> requestedCommentary)
        {
            if (requestedCommentary is null)
            {
                return await _dataContext.Commentary.Where(comm => comm.IsLive).OrderByDescending(x => x.CommentaryStart).ToListAsync();
            }
            return await _dataContext.Commentary.Where(comm => comm.IsLive && requestedCommentary.Contains(comm.CommentaryID)).OrderByDescending(x => x.CommentaryStart).ToListAsync();
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

        public Task<int> CountAllCommentaryAsync(bool liveOnly)
        {
            if (liveOnly)
            {
                return _dataContext.Commentary.Where(comm => comm.IsLive).CountAsync();
            }
            return _dataContext.Commentary.CountAsync();
        }

        public async Task<ICollection<int>> GetSpecificIDs(int skip, int take, bool liveOnly)
        {
            if (liveOnly)
            {
                return await _dataContext.Commentary
                    .Where(x => x.IsLive)
                    .OrderByDescending(x => x.CommentaryStart)
                    .Skip(skip)
                    .Take(take)
                    .Select(y => y.CommentaryID)
                    .ToListAsync();
            }
            else
            {
                return await _dataContext.Commentary
                    .OrderByDescending(x => x.CommentaryStart)
                    .Skip(skip)
                    .Take(take)
                    .Select(y => y.CommentaryID)
                    .ToListAsync();
            }
        }
    }
}
