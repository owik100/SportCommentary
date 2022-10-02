using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportCommentary.Data;
using SportCommentary.Repository.Interfaces;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Repository
{
    public class EventsRepository : IEventsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dataContext;
        public EventsRepository(IDbContextFactory<ApplicationDbContext> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateEventAsync(Event eventType)
        {
            using var context = _dataContext.CreateDbContext();

            await context.Event.AddAsync(eventType);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> DeleteEventTypeAsync(Event eventType)
        {
            using var context = _dataContext.CreateDbContext();

            context.Remove(eventType);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<ICollection<Event>> GetAllEventsAsync()
        {
            using var context = _dataContext.CreateDbContext();

            return await context
                .Event
                .Include(ev => ev.SportType)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            using var context = _dataContext.CreateDbContext();

            return await context.Event.FirstOrDefaultAsync(spType => spType.EventID == eventId);
        }

        public async Task<ICollection<Event>> GetEventBySportIdAsync(int sportId)
        {
            using var context = _dataContext.CreateDbContext();

            return await context.Event.Where(ev => ev.SportTypeID == sportId || ev.SportTypeID == null).ToListAsync();
        }

        public async Task<bool> UpdateEventAsync(Event eventType)
        {
            using var context = _dataContext.CreateDbContext();

            context.Event.Update(eventType);
            return await context.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
