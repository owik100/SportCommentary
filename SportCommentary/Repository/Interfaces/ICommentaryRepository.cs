﻿using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository.Interfaces
{
    public interface ICommentaryRepository
    {
        /// <summary>
        /// Return all Commentary
        /// </summary>
        /// <returns>Commentary</returns>
        Task<ICollection<Commentary>> GetAllCommentaryAsync();
        /// <summary>
        /// Return all Commentary that are live
        /// </summary>
        /// <returns>Commentary</returns>
        Task<ICollection<Commentary>> GetAllCommentaryLiveAsync();
        /// <summary>
        /// Return a Commentary record
        /// </summary>
        /// <param name="commentaryId"></param>
        /// <returns>Commentary</returns>
        Task<Commentary> GetCommentaryByIdAsync(int commentaryId);
        /// <summary>
        /// Add a new record for Commentary
        /// </summary>
        /// <param name="commentary"></param>
        /// <returns>bool</returns>
        Task<bool> CreateCommentaryAsync(Commentary commentary);
        /// <summary>
        /// Update a record in db
        /// </summary>
        /// <param name="commentary"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateCommentaryAsync(Commentary commentary);
        /// <summary>
        /// Delete a record from db
        /// </summary>
        /// <param name="commentary"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentaryAsync(Commentary commentary);
        /// <summary>
        /// Return True/False if record exist
        /// </summary>
        /// <param name="CommentaryName"></param>
        /// <returns>bool</returns>
        Task<bool> CommentaryExistAsync(string CommentaryName);
    }
}
