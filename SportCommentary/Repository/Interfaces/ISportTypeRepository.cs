using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository.Interfaces
{
    public interface ISportTypeRepository
    {
        /// <summary>
        /// Return all SportTypes
        /// </summary>
        /// <returns>SportType</returns>
        Task<ICollection<SportType>> GetAllSportTypesAsync();
        /// <summary>
        /// Return a SportyType record
        /// </summary>
        /// <param name="sportTypeId"></param>
        /// <returns>SportType</returns>
        Task<SportType> GetSportTypeByIdAsync(int sportTypeId);
        /// <summary>
        /// Return True/False if record exist
        /// </summary>
        /// <param name="SportTypeName"></param>
        /// <returns>bool</returns>
        Task<bool> SportTypeExistAsync(string SportTypeName);
        /// <summary>
        /// Add a new record for sportType
        /// </summary>
        /// <param name="sportType"></param>
        /// <returns>bool</returns>
        Task<bool> CreateSportTypeAsync(SportType sportType);
        /// <summary>
        /// Update a record in db
        /// </summary>
        /// <param name="sportType"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateSportTypeAsync(SportType sportType);
        /// <summary>
        /// Delete a record from db
        /// </summary>
        /// <param name="sportType"></param>
        /// <returns></returns>
        Task<bool> DeleteSportTypeAsync(SportType sportType);
    }
}
