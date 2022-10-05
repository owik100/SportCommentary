using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SportCommentary.Helpers;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO.Commentary;
using SportCommentaryDataAccess.DTO.Event;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using static MudBlazor.CategoryTypes;

namespace SportCommentary.Service
{
    public class CommentaryService : ICommentaryService
    {
        private readonly ICommentaryRepository _commentaryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IPaginationHelper _paginationHelper;
        public CommentaryService(IMapper mapper, ICommentaryRepository commentaryRepository, IMemoryCache memoryCache, IPaginationHelper paginationHelper)
        {
            _mapper = mapper;
            _commentaryRepository = commentaryRepository;
            _memoryCache = memoryCache;
            _paginationHelper = paginationHelper;
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
            _memoryCache.Remove("AllSportTypes");
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
        public async Task<ServiceResponse<PagedList<CommentaryDTO>>> GetAllCommentaryAsync(int pageNumber, int pageSize)
        {
            ServiceResponse<PagedList<CommentaryDTO>> _response = new();
            List<CommentaryDTO> responseCommentaryDTOList = new List<CommentaryDTO>();
            try
            {

                if (pageNumber <= 0)
                    pageNumber = 1;

                int skip = (pageNumber - 1) * pageSize;
                int take = pageSize;
                int count = await _commentaryRepository.CountAllCommentaryAsync(false);

                int totalPages = _paginationHelper.CalculateTotalPages(count, pageSize);

                List<int> requestIDs = (List<int>)await _commentaryRepository.GetSpecificIDs(skip, take, false);



                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                   .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                   .SetSize(1024);

                List<CommentaryDTO> CommentaryDTOList = new List<CommentaryDTO>();
                if (!_memoryCache.TryGetValue("AllCommentary", out CommentaryDTOList))
                {
                    CommentaryDTOList = new List<CommentaryDTO>();
                    ICollection<Commentary> Comments = await _commentaryRepository.GetAllCommentaryAsync(requestIDs);
                    foreach (var item in Comments)
                    {
                        CommentaryDTOList.Add(_mapper.Map<CommentaryDTO>(item));
                    }
                }
                else
                {
                    foreach (var id in requestIDs)
                    {
                        if(!CommentaryDTOList.Any(x => x.CommentaryID == id)){
                            Commentary newComm = await _commentaryRepository.GetCommentaryByIdAsync(id);
                            CommentaryDTOList.Add(_mapper.Map<CommentaryDTO>(newComm));
                        }
                    }
                }
                _memoryCache.Set("AllCommentary", CommentaryDTOList, cacheOptions);

                foreach (var item in CommentaryDTOList)
                {
                    if (requestIDs.Contains(item.CommentaryID))
                    {
                        responseCommentaryDTOList.Add(item);
                    }
                }

                responseCommentaryDTOList = responseCommentaryDTOList.OrderByDescending(x => x.CommentaryStart).ToList();
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = new PagedList<CommentaryDTO>() { CurrentPage = pageNumber, Items = responseCommentaryDTOList, PageSize = pageSize, TotalItems = count };

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

        public async Task<ServiceResponse<PagedList<CommentaryDTO>>> GetAllCommentaryLiveAsync(int pageNumber = 1, int pageSize = 10)
        {
            ServiceResponse<PagedList<CommentaryDTO>> _response = new();
            List<CommentaryDTO> responseCommentaryDTOList = new List<CommentaryDTO>();
            try
            {
                if (pageNumber <= 0)
                    pageNumber = 1;

                int skip = (pageNumber - 1) * pageSize;
                int take = pageSize;
                int count = await _commentaryRepository.CountAllCommentaryAsync(true);

                int totalPages = _paginationHelper.CalculateTotalPages(count, pageSize);

                List<int> requestIDs = (List<int>)await _commentaryRepository.GetSpecificIDs(skip, take, true);

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                  .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                  .SetSize(1024);

                List<CommentaryDTO> CommentaryDTOList = new List<CommentaryDTO>();
                if (!_memoryCache.TryGetValue("AllCommentary", out CommentaryDTOList))
                {
                    CommentaryDTOList = new List<CommentaryDTO>();
                    ICollection<Commentary> Comments = await _commentaryRepository.GetAllCommentaryLiveAsync(requestIDs);
                    foreach (var item in Comments)
                    {
                        CommentaryDTOList.Add(_mapper.Map<CommentaryDTO>(item));
                    }
                }
                else
                {
                    foreach (var id in requestIDs)
                    {
                        if (!CommentaryDTOList.Any(x => x.CommentaryID == id))
                        {
                            Commentary newComm = await _commentaryRepository.GetCommentaryByIdAsync(id);
                            CommentaryDTOList.Add(_mapper.Map<CommentaryDTO>(newComm));
                        }
                    }
                }
                _memoryCache.Set("AllCommentary", CommentaryDTOList, cacheOptions);

                foreach (var item in CommentaryDTOList.ToList())
                {
                    if (!requestIDs.Contains(item.CommentaryID)){
                        CommentaryDTOList.Remove(item);
                    }
                }

                foreach (var item in CommentaryDTOList)
                {
                    if (requestIDs.Contains(item.CommentaryID))
                    {
                        responseCommentaryDTOList.Add(item);
                    }
                }

                responseCommentaryDTOList = responseCommentaryDTOList.OrderByDescending(x => x.CommentaryStart).ToList();
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = new PagedList<CommentaryDTO>() { CurrentPage = pageNumber, Items = CommentaryDTOList, PageSize = pageSize, TotalItems = count };

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
