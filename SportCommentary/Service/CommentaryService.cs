using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Commentary;
using SportCommentaryDataAccess.DTO.Event;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using System.Reflection;

namespace SportCommentary.Service
{
    public class CommentaryService : ICommentaryService
    {
        private readonly ICommentaryRepository _commentaryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public CommentaryService(IMapper mapper, ICommentaryRepository commentaryRepository, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _commentaryRepository = commentaryRepository;
            _memoryCache = memoryCache;
        }
        public async Task<ServiceResponse<CommentaryDTO>> AddCommentaryAsync(CreateCommentaryDTO createCommentaryDTO)
        {
            ServiceResponse<CommentaryDTO> response = new();
            try
            {
                if (await _commentaryRepository.CommentaryExistAsync(createCommentaryDTO.Caption))
                {
                    response.Message = "Ta relacja już istnieje";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }

                createCommentaryDTO.CommentaryStart = DateTime.Now;
                createCommentaryDTO.IsLive = true;
                var newCommentary = _mapper.Map<Commentary>(createCommentaryDTO);

                if (!await _commentaryRepository.CreateCommentaryAsync(newCommentary))
                {
                    response.Message = "Błąd przy dodawaniu relacji";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<CommentaryDTO>(newCommentary);
                response.Message = "Created";

                List<CommentaryDTO> CommentaryDTOList = new List<CommentaryDTO>();
                if (_memoryCache.TryGetValue("AllCommentary", out CommentaryDTOList))
                {
                    if (CommentaryDTOList != null)
                    {
                        CommentaryDTOList.Add(response.Data);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllEvents", CommentaryDTOList, cacheOptions);
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

        public async Task<ServiceResponse<string>> DeleteCommentary(int Id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                Commentary _existingCommentary = await _commentaryRepository.GetCommentaryByIdAsync(Id);

                if (_existingCommentary == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono relacji do usnięcia";
                    _response.Data = null;
                    return _response;
                }

                if (!await _commentaryRepository.DeleteCommentaryAsync(_existingCommentary))
                {
                    _response.Success = false;
                    _response.Message = "Nie udało się usunąć relacji";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "Deleted";

                List<CommentaryDTO> CommentaryDTOList = new List<CommentaryDTO>();
                if (_memoryCache.TryGetValue("AllCommentary", out CommentaryDTOList))
                {
                    if (CommentaryDTOList != null)
                    {
                        CommentaryDTOList.RemoveAll(x => x.CommentaryID == Id);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllCommentary", CommentaryDTOList, cacheOptions);
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

        public async Task<ServiceResponse<List<CommentaryDTO>>> GetAllCommentaryAsync()
        {
            ServiceResponse<List<CommentaryDTO>> _response = new();
            try
            {
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                   .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                   .SetSize(1024);

                List<CommentaryDTO> CommentaryDTOList = new List<CommentaryDTO>();
                if (!_memoryCache.TryGetValue("AllCommentary", out CommentaryDTOList))
                {
                    CommentaryDTOList = new List<CommentaryDTO>();
                    ICollection<Commentary> Comments = await _commentaryRepository.GetAllCommentaryAsync();
                    foreach (var item in Comments)
                    {
                        CommentaryDTOList.Add(_mapper.Map<CommentaryDTO>(item));
                    }
                    _memoryCache.Set("AllCommentary", CommentaryDTOList, cacheOptions);
                }
                    

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = CommentaryDTOList;

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

        public async Task<ServiceResponse<List<CommentaryDTO>>> GetAllCommentaryLiveAsync()
        {
            ServiceResponse<List<CommentaryDTO>> _response = new();
            try
            {
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                  .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                  .SetSize(1024);

                List<CommentaryDTO> CommentaryDTOList = new List<CommentaryDTO>();
                if (!_memoryCache.TryGetValue("LiveCommentary", out CommentaryDTOList))
                {
                    CommentaryDTOList = new List<CommentaryDTO>();
                    ICollection<Commentary> Comments = await _commentaryRepository.GetAllCommentaryLiveAsync();
                    foreach (var item in Comments)
                    {
                        CommentaryDTOList.Add(_mapper.Map<CommentaryDTO>(item));
                    }
                    _memoryCache.Set("LiveCommentary", CommentaryDTOList, cacheOptions);
                }

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = CommentaryDTOList;

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

        public async Task<ServiceResponse<CommentaryDTO>> GetByIdAsync(int Id)
        {
            ServiceResponse<CommentaryDTO> _response = new();
            try
            {
                Commentary commentary = await _commentaryRepository.GetCommentaryByIdAsync(Id);

                if (commentary == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono relacji";
                    return _response;
                }

                CommentaryDTO commentaryDTO = _mapper.Map<CommentaryDTO>(commentary);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = commentaryDTO;

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

        public async Task<ServiceResponse<CommentaryDTO>> UpdateCommentaryAsync(UpdateCommentaryDTO updateCommentaryDTO)
        {
            ServiceResponse<CommentaryDTO> _response = new();

            try
            {

                Commentary _existingCommentary = await _commentaryRepository.GetCommentaryByIdAsync(updateCommentaryDTO.CommentaryID);

                if (_existingCommentary == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono relacji do zaktualizowania";
                    _response.Data = null;
                    return _response;

                }
                _mapper.Map(updateCommentaryDTO, _existingCommentary);

                if (!await _commentaryRepository.UpdateCommentaryAsync(_existingCommentary))
                {
                    _response.Success = false;
                    _response.Message = "Błąd przy aktualizacji sportu";
                    _response.Data = null;
                    return _response;
                }

                CommentaryDTO commentaryDTO = _mapper.Map<CommentaryDTO>(_existingCommentary);
                _response.Success = true;
                _response.Message = "Updated";
                _response.Data = commentaryDTO;

                List<CommentaryDTO> CommentaryCache = new List<CommentaryDTO>();
                if (_memoryCache.TryGetValue("AllCommentary", out CommentaryCache))
                {
                    if (CommentaryCache != null)
                    {
                        CommentaryDTO oldCommentaryInCache = CommentaryCache.Find(x => x.CommentaryID == updateCommentaryDTO.CommentaryID);
                        foreach (PropertyInfo property in typeof(CommentaryDTO).GetProperties().Where(p => p.CanWrite))
                        {
                            property.SetValue(oldCommentaryInCache, property.GetValue(commentaryDTO, null), null);
                        }
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllCommentary", CommentaryCache, cacheOptions);
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
