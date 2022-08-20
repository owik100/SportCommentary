using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SportType;

namespace SportCommentary.Service.Interfaces
{
    public interface ISportTypeService
    {
        /// <summary>
        /// Return all SportTypes.
        /// </summary>
        /// <returns>List Of SportTypeDTO</returns>
        Task<ServiceResponse<List<SportTypeDTO>>> GetAllSportTypesAsync();
        /// <summary>
        /// Return SportTypeDTO record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>SportTypeDTO</returns>
        Task<ServiceResponse<SportTypeDTO>> GetByIdAsync(int Id);
        /// <summary>
        /// Add new SportType record in db
        /// </summary>
        /// <param name="createSportTypeDTO"></param>
        /// <returns>SportTypeDTO</returns>
        Task<ServiceResponse<SportTypeDTO>> AddSportTypeAsync(CreateSportTypeDTO createSportTypeDTO);
        /// <summary>
        /// Update SportType record
        /// </summary>
        /// <param name="updateSportTypeDTO"></param>
        /// <returns>CompanyDto</returns>
        Task<ServiceResponse<SportTypeDTO>> UpdateSportTypeAsync(UpdateSportTypeDTO updateSportTypeDTO);
        /// <summary>
        /// Remove SportType record
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>bool</returns>
        Task<ServiceResponse<string>> DeleteSportType(int Id);
    }
}
