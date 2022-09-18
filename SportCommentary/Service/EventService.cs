using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Event;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using System.Reflection;

namespace SportCommentary.Service
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public EventService(IMapper mapper, IEventsRepository eventRepo, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _eventRepo = eventRepo;
            _memoryCache = memoryCache;
        }
        public async Task<ServiceResponse<EventDTO>> AddEventAsync(CreateEventDTO createEventDTO)
        {
            ServiceResponse<EventDTO> response = new();
            try
            {
                var newEvent = _mapper.Map<Event>(createEventDTO);

                if (!await _eventRepo.CreateEventAsync(newEvent))
                {
                    response.Message = "Błąd przy dodawaniu wydarzenia";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<EventDTO>(newEvent);
                response.Message = "Created";

                List<EventDTO> EventDTOList = new List<EventDTO>();
                if (_memoryCache.TryGetValue("AllEvents", out EventDTOList))
                {
                    if (EventDTOList != null)
                    {
                        EventDTOList.Add(response.Data);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllEvents", EventDTOList, cacheOptions);
                    }
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Wystąpił nieoczekiwany błąd";
                response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };

            }
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteEventType(int Id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                Event _existingEvent = await _eventRepo.GetEventByIdAsync(Id);

                if (_existingEvent == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono wydarzenia do usnięcia";
                    _response.Data = null;
                    return _response;
                }

                if (!await _eventRepo.DeleteEventTypeAsync(_existingEvent))
                {
                    _response.Success = false;
                    _response.Message = "Nie udało się usunąć wydarzenia";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "Deleted";

                List<EventDTO> EventDTOList = new List<EventDTO>();
                if (_memoryCache.TryGetValue("AllEvents", out EventDTOList))
                {
                    if (EventDTOList != null)
                    {
                        EventDTOList.RemoveAll(x => x.EventID == Id);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllEvents", EventDTOList, cacheOptions);
                    }
                }

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Wystąpił nieoczekiwany błąd;";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<List<EventDTO>>> GetAllEventsAsync()
        {
            ServiceResponse<List<EventDTO>> _response = new();
            try
            {
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetSize(1024);

                List<EventDTO> eventDTOList = new List<EventDTO>();
                if (!_memoryCache.TryGetValue("AllEvents", out eventDTOList))
                {
                    eventDTOList = new List<EventDTO>();
                    ICollection<Event> Events = await _eventRepo.GetAllEventsAsync();
                    foreach (var item in Events)
                    {
                        eventDTOList.Add(_mapper.Map<EventDTO>(item));
                    }
                    _memoryCache.Set("AllEvents", eventDTOList, cacheOptions);
                }
                   
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = eventDTOList;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Wystąpił nieoczekiwany błąd";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<EventDTO>> GetByIdAsync(int Id)
        {
            ServiceResponse<EventDTO> _response = new();
            try
            {
                Event eventType = await _eventRepo.GetEventByIdAsync(Id);

                if (eventType == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono wydarzenia";
                    return _response;
                }

                EventDTO eventDTO = _mapper.Map<EventDTO>(eventType);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = eventDTO;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Wystąpił nieoczekiwany błąd";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<EventDTO>> UpdateEventAsync(UpdateEventDTO updateEventDTO)
        {
            ServiceResponse<EventDTO> _response = new();

            try
            {

                Event _existingEvent = await _eventRepo.GetEventByIdAsync(updateEventDTO.EventID);

                if (_existingEvent == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono wydarzenia do zaktualizowania";
                    _response.Data = null;
                    return _response;

                }
                _mapper.Map(updateEventDTO, _existingEvent);

                if (!await _eventRepo.UpdateEventAsync(_existingEvent))
                {
                    _response.Success = false;
                    _response.Message = "Błąd przy aktualizacji wydarzenia";
                    _response.Data = null;
                    return _response;
                }

                EventDTO eventTypeDTO = _mapper.Map<EventDTO>(_existingEvent);
                _response.Success = true;
                _response.Message = "Updated";
                _response.Data = eventTypeDTO;

                List<EventDTO> EventsCache = new List<EventDTO>();
                if (_memoryCache.TryGetValue("AllEvents", out EventsCache))
                {
                    if (EventsCache != null)
                    {
                        EventDTO oldEventInCache = EventsCache.Find(x => x.EventID == eventTypeDTO.EventID);
                        foreach (PropertyInfo property in typeof(EventDTO).GetProperties().Where(p => p.CanWrite))
                        {
                            property.SetValue(oldEventInCache, property.GetValue(eventTypeDTO, null), null);
                        }
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllEvents", EventsCache, cacheOptions);
                    }
                }

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Wystąpił nieoczekiwany błąd";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
    }
}
