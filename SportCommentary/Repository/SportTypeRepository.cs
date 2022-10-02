using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SportCommentary.Repository
{
    public class SportTypeRepository : ISportTypeRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dataContext;
        public SportTypeRepository(IDbContextFactory<ApplicationDbContext> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> SportTypeExistAsync(string SportTypeName)
        {
            using var context = _dataContext.CreateDbContext();

            return await context.SportType.AnyAsync(SportType => SportType.Name == SportTypeName);
        }

        public async Task<bool> CreateSportTypeAsync(SportType sportType)
        {
            using var context = _dataContext.CreateDbContext();

            await context.SportType.AddAsync(sportType);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }
        public async Task<bool> UpdateSportTypeAsync(SportType sportType)
        {
            using var context = _dataContext.CreateDbContext();

            context.SportType.Update(sportType);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> DeleteSportTypeAsync(SportType sportType)
        {
            using var context = _dataContext.CreateDbContext();

            context.Remove(sportType);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<ICollection<SportType>> GetAllSportTypesAsync()
        {
            using var context = _dataContext.CreateDbContext();

            return await context
                .SportType
                .Include(x => x.Commentaries)
                .ToListAsync();
        }

        public async Task<SportType> GetSportTypeByIdAsync(int sportTypeId)
        {
            using var context = _dataContext.CreateDbContext();

            return await context.SportType.FirstOrDefaultAsync(spType => spType.SportTypeID == sportTypeId);
        }
    }
}
