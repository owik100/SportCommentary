using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SingleCommentary;

namespace SportCommentary.Service.Interfaces
{
    public interface ISingleCommentaryService
    {
        /// <summary>
        /// Return SingleCommentDTO collection in commentary.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>List<SingleCommentaryDTO>></returns>
        Task<ServiceResponse<List<SingleCommentDTO>>> GetAllSingleCommentsInCommentaryAsync(int Id);
        /// <summary>
        /// Return SingleCommentDTO record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>SingleCommentDTO</returns>
        /// 
        Task<ServiceResponse<SingleCommentDTO>> GetByIdAsync(int Id);
        /// <summary>
        /// Add new CreateSingleCommentaryDTO record in db
        /// </summary>
        /// <param name="createSingleCommentaryDTO"></param>
        /// <returns>SportTypeDTO</returns>
        Task<ServiceResponse<SingleCommentDTO>> AddSingleCommentaryAsync(CreateSingleCommentaryDTO createSingleCommentaryDTO);
        /// <summary>
        /// Remove SportTypeDTO record
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>bool</returns>
        Task<ServiceResponse<string>> DeleteSingleCommentary(int Id);
    }
}
