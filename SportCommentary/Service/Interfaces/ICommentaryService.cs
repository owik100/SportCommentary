using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Commentary;

namespace SportCommentary.Service.Interfaces
{
    public interface ICommentaryService
    {
        /// <summary>
        /// Return all commentary.
        /// </summary>
        /// <returns>List Of CommentaryDTO</returns>
        Task<ServiceResponse<PagedList<CommentaryDTO>>> GetAllCommentaryAsync(int pageNumber = 1, int pageSize = 10);
        /// <summary>
        /// Return CommentaryDTO record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>CommentaryDTO</returns>
        Task<ServiceResponse<CommentaryDTO>> GetByIdAsync(int Id);
        /// <summary>
        /// Add new Commentary record in db
        /// </summary>
        /// <param name="createCommentaryDTO"></param>
        /// <returns>CommentaryDTO</returns>
        Task<ServiceResponse<CommentaryDTO>> AddCommentaryAsync(CreateCommentaryDTO createCommentaryDTO);
        /// <summary>
        /// Update Commentary record
        /// </summary>
        /// <param name="updateSportTypeDTO"></param>
        /// <returns>CommentaryDTO</returns>
        Task<ServiceResponse<CommentaryDTO>> UpdateCommentaryAsync(UpdateCommentaryDTO updateCommentaryDTO);
        /// <summary>
        /// Remove Commentary record
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>bool</returns>
        Task<ServiceResponse<string>> DeleteCommentary(int Id);
        /// <summary>
        /// Return all live CommentaryDTO record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>SportTypeDTO</returns>
        /// 
        Task<ServiceResponse<PagedList<CommentaryDTO>>> GetAllCommentaryLiveAsync(int pageNumber = 1, int pageSize = 10);
    }
}
