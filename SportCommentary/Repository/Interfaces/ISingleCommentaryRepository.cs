using SportCommentaryDataAccess.DTO.Commentary;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.SingleCommentary;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using System.Collections.Generic;

namespace SportCommentary.Repository.Interfaces
{
    public interface ISingleCommentaryRepository
    {
        /// <summary>
        /// Return SingleComment collection in commentary.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>List<SingleCommentaryDTO>></returns>
        Task<ICollection<SingleComment>> GetByCommentaryIdAsync(int Id);
        /// <summary>
        /// Return SingleComment record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>SingleCommentaryDTO</returns>
        Task<SingleComment> GetByIdAsync(int Id);
        /// <summary>
        /// Add a new record for SingleComment
        /// </summary>
        /// <param name="singleComment"></param>
        /// <returns>bool</returns>
        Task<bool> CreateSingleCommentaryAsync(SingleComment singleComment);
        /// <summary>
        /// Delete a record from db
        /// </summary>
        /// <param name="singleComment"></param>
        /// <returns></returns>
        Task<bool> DeleteSingleCommentAsync(SingleComment singleComment);

    }
}
