using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository.Interfaces
{
    public interface IEventsRepository
    {
        /// <summary>
        /// Return all Evenets
        /// </summary>
        /// <returns>Event</returns>
        Task<ICollection<Event>> GetAllEventsAsync();
        /// <summary>
        /// Return a Event record
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>Event</returns>
        Task<Event> GetEventByIdAsync(int eventId);
        /// <summary>
        /// Add a new record for event
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns>bool</returns>
        Task<bool> CreateEventAsync(Event eventType);
        /// <summary>
        /// Update a record in db
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateEventAsync(Event eventType);
        /// <summary>
        /// Delete a record from db
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task<bool> DeleteEventTypeAsync(Event eventType);
        /// <summary>
        /// Get events by sportID
        /// </summary>
        /// <param name="sportId"></param>
        /// <returns>Event</returns>
        Task<ICollection<Event>> GetEventBySportIdAsync(int sportId);
    }
}
