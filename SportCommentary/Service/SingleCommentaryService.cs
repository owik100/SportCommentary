using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Event;
using SportCommentaryDataAccess.DTO.SingleCommentary;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Service
{
    public class SingleCommentaryService : ISingleCommentaryService
    {
        private readonly ISingleCommentaryRepository _singleCommRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public SingleCommentaryService(IMapper mapper, ISingleCommentaryRepository singleCommRepo, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _singleCommRepo = singleCommRepo;
            _memoryCache = memoryCache;
        }
        public async Task<ServiceResponse<SingleCommentDTO>> AddSingleCommentaryAsync(CreateSingleCommentaryDTO createSingleCommentaryDTO)
        {
            ServiceResponse<SingleCommentDTO> response = new();
            try
            {
                var newSingleComment = _mapper.Map<SingleComment>(createSingleCommentaryDTO);

                if (!await _singleCommRepo.CreateSingleCommentaryAsync(newSingleComment))
                {
                    response.Message = "Błąd przy dodawaniu komentarza";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<SingleCommentDTO>(newSingleComment);
                response.Message = "Created";

                //List<EventDTO> EventDTOList = new List<EventDTO>();
                //if (_memoryCache.TryGetValue("AllEvents", out EventDTOList))
                //{
                //    if (EventDTOList != null)
                //    {
                //        EventDTOList.Add(response.Data);
                //        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                //            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                //            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                //            .SetSize(1024);
                //        _memoryCache.Set("AllEvents", EventDTOList, cacheOptions);
                //    }
                //}

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

        public async Task<ServiceResponse<string>> DeleteSingleCommentary(int Id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                SingleComment _existingSingleComment = await _singleCommRepo.GetByIdAsync(Id);

                if (_existingSingleComment == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono komentarza do usnięcia";
                    _response.Data = null;
                    return _response;
                }

                if (!await _singleCommRepo.DeleteSingleCommentAsync(_existingSingleComment))
                {
                    _response.Success = false;
                    _response.Message = "Nie udało się usunąć komentarza";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "Deleted";

                //List<EventDTO> EventDTOList = new List<EventDTO>();
                //if (_memoryCache.TryGetValue("AllEvents", out EventDTOList))
                //{
                //    if (EventDTOList != null)
                //    {
                //        EventDTOList.RemoveAll(x => x.EventID == Id);
                //        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                //            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                //            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                //            .SetSize(1024);
                //        _memoryCache.Set("AllEvents", EventDTOList, cacheOptions);
                //    }
                //}

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

        public async Task<ServiceResponse<List<SingleCommentDTO>>> GetAllSingleCommentsInCommentaryAsync(int Id)
        {
            ServiceResponse<List<SingleCommentDTO>> _response = new();
            try
            {
                //MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                //    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                //    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                //    .SetSize(1024);

                List<SingleCommentDTO> singleCommentDTOList = new List<SingleCommentDTO>();
                //if (!_memoryCache.TryGetValue("AllEvents", out eventDTOList))
                //{
                    singleCommentDTOList = new List<SingleCommentDTO>();
                    ICollection<SingleComment> SingleComments = await _singleCommRepo.GetByCommentaryIdAsync(Id);
                    foreach (var item in SingleComments)
                    {
                        singleCommentDTOList.Add(_mapper.Map<SingleCommentDTO>(item));
                    }
                   // _memoryCache.Set("AllEvents", singleCommentDTOList, cacheOptions);
                //}

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = singleCommentDTOList;

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

        public async Task<ServiceResponse<SingleCommentDTO>> GetByIdAsync(int Id)
        {
            ServiceResponse<SingleCommentDTO> _response = new();
            try
            {
                SingleComment eventType = await _singleCommRepo.GetByIdAsync(Id);

                if (eventType == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono komentarza";
                    return _response;
                }

                SingleCommentDTO singleCommentDTO = _mapper.Map<SingleCommentDTO>(eventType);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = singleCommentDTO;

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
