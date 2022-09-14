using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Event;

namespace SportCommentary.Service.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Return all Events.
        /// </summary>
        /// <returns>List Of EventDTO</returns>
        Task<ServiceResponse<List<EventDTO>>> GetAllEventsAsync();
        /// <summary>
        /// Return EventDTO record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>EventDTO</returns>
        Task<ServiceResponse<EventDTO>> GetByIdAsync(int Id);
        /// <summary>
        /// Add new Event record in db
        /// </summary>
        /// <param name="createEventDTO"></param>
        /// <returns>EventDTO</returns>
        Task<ServiceResponse<EventDTO>> AddEventAsync(CreateEventDTO createEventDTO);
        /// <summary>
        /// Update Event record
        /// </summary>
        /// <param name="updateEventDTO"></param>
        /// <returns>EventDTO</returns>
        Task<ServiceResponse<EventDTO>> UpdateEventAsync(UpdateEventDTO updateEventDTO);
        /// <summary>
        /// Remove Event record
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>bool</returns>
        Task<ServiceResponse<string>> DeleteEventType(int Id);
    }
}
