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
        private readonly ApplicationDbContext _dataContext;
        public SportTypeRepository(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> SportTypeExistAsync(string SportTypeName)
        {
            return await _dataContext.SportType.AnyAsync(SportType => SportType.Name == SportTypeName);
        }

        public async Task<bool> CreateSportTypeAsync(SportType sportType)
        {
            await _dataContext.SportType.AddAsync(sportType);
            return await Save();
        }
        public async Task<bool> UpdateSportTypeAsync(SportType sportType)
        {
            _dataContext.SportType.Update(sportType);
            return await Save();
        }

        public async Task<bool> DeleteSportTypeAsync(SportType sportType)
        {
            _dataContext.Remove(sportType);
            return await Save();
        }

        public async Task<ICollection<SportType>> GetAllSportTypesAsync()
        {
            return await _dataContext
                .SportType
                .Include(x => x.Commentaries)
                .ToListAsync();
        }

        public async Task<SportType> GetSportTypeByIdAsync(int sportTypeId)
        {
            return await _dataContext.SportType.FirstOrDefaultAsync(spType => spType.SportTypeID == sportTypeId);
        }

        
        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
