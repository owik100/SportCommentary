using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository
{
    public class EventsRepository : IEventsRepository
    {
        private readonly ApplicationDbContext _dataContext;
        public EventsRepository(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateEventAsync(Event eventType)
        {
            await _dataContext.Event.AddAsync(eventType);
            return await Save();
        }

        public async Task<bool> DeleteEventTypeAsync(Event eventType)
        {
            _dataContext.Remove(eventType);
            return await Save();
        }

        public async Task<ICollection<Event>> GetAllEventsAsync()
        {
            return await _dataContext.Event.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            return await _dataContext.Event.FirstOrDefaultAsync(spType => spType.EventID == eventId);
        }

        public async Task<ICollection<Event>> GetEventBySportIdAsync(int sportId)
        {
            return await _dataContext.Event.Where(ev => ev.SportTypeID == sportId || ev.SportTypeID == null).ToListAsync();
        }

        public async Task<bool> UpdateEventAsync(Event eventType)
        {
            _dataContext.Event.Update(eventType);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
