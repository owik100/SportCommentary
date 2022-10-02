using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess.Entities;
using System.Linq;

namespace SportCommentary.Repository
{
    public class CommentaryRepository : ICommentaryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dataContextFactory;
        public CommentaryRepository(IDbContextFactory<ApplicationDbContext> dataContext)
        {
            _dataContextFactory = dataContext;
        }

        public async Task<ICollection<Commentary>> GetAllCommentaryAsync(List<int> requestedCommentary)
        {
            using var context = _dataContextFactory.CreateDbContext();

            if (requestedCommentary is null)
            {
                return await context.Commentary.OrderByDescending(x => x.CommentaryStart).ToListAsync();
            }
            return await context.Commentary.Where(comm => requestedCommentary.Contains(comm.CommentaryID)).OrderByDescending(x => x.CommentaryStart).ToListAsync();
        }

        public async Task<ICollection<Commentary>> GetAllCommentaryLiveAsync(List<int> requestedCommentary)
        {
            using var context = _dataContextFactory.CreateDbContext();

            if (requestedCommentary is null)
            {
                return await context.Commentary.Where(comm => comm.IsLive).OrderByDescending(x => x.CommentaryStart).ToListAsync();
            }
            return await context.Commentary.Where(comm => comm.IsLive && requestedCommentary.Contains(comm.CommentaryID)).OrderByDescending(x => x.CommentaryStart).ToListAsync();
        }

        public async Task<bool> CommentaryExistAsync(string CommentaryName)
        {
            using var context = _dataContextFactory.CreateDbContext();

            return await context.Commentary.AnyAsync(Comm => Comm.Caption == CommentaryName);
        }

        public async Task<Commentary> GetCommentaryByIdAsync(int commentaryId)
        {
            using var context = _dataContextFactory.CreateDbContext();

            return await context.Commentary.FirstOrDefaultAsync(comm => comm.CommentaryID == commentaryId);
        }

        public async Task<bool> CreateCommentaryAsync(Commentary commentary)
        {
            using var context = _dataContextFactory.CreateDbContext();

            await context.Commentary.AddAsync(commentary);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateCommentaryAsync(Commentary commentary)
        {
            using var context = _dataContextFactory.CreateDbContext();

            context.Commentary.Update(commentary);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> DeleteCommentaryAsync(Commentary commentary)
        {
            using var context = _dataContextFactory.CreateDbContext();

            context.Remove(commentary);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<int> CountAllCommentaryAsync(bool liveOnly)
        {
            using var context = _dataContextFactory.CreateDbContext();

            if (liveOnly)
            {
                return await context.Commentary.Where(comm => comm.IsLive).CountAsync();
            }
                return await context.Commentary.CountAsync();
        }

        public async Task<ICollection<int>> GetSpecificIDs(int skip, int take, bool liveOnly)
        {
            using var context = _dataContextFactory.CreateDbContext();

            if (liveOnly)
            {
                return await context.Commentary
                    .Where(x => x.IsLive)
                    .OrderByDescending(x => x.CommentaryStart)
                    .Skip(skip)
                    .Take(take)
                    .Select(y => y.CommentaryID)
                    .ToListAsync();
            }
            else
            {
                return await context.Commentary
                    .OrderByDescending(x => x.CommentaryStart)
                    .Skip(skip)
                    .Take(take)
                    .Select(y => y.CommentaryID)
                    .ToListAsync();
            }
        }
    }
}
