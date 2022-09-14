using AutoMapper;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Event;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Service
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _eventRepo;
        private readonly IMapper _mapper;
        public EventService(IMapper mapper, IEventsRepository eventRepo)
        {
            _mapper = mapper;
            _eventRepo = eventRepo;
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
                ICollection<Event> Events = await _eventRepo.GetAllEventsAsync();

                List<EventDTO> eventDTOList = new List<EventDTO>();

                foreach (var item in Events)
                {
                    eventDTOList.Add(_mapper.Map<EventDTO>(item));
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
