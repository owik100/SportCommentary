using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO;
using SportCommentaryDataAccess.DTO.Commentary;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using System.Reflection;

namespace SportCommentary.Service
{
    public class SportTypeService : ISportTypeService
    {
        private readonly ISportTypeRepository _sportTypeRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public SportTypeService(IMapper mapper, ISportTypeRepository sportTypeRepo, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _sportTypeRepo = sportTypeRepo;
            _memoryCache = memoryCache;
        }
        public async Task<ServiceResponse<SportTypeDTO>> AddSportTypeAsync(CreateSportTypeDTO createSportTypeDTO)
        {
            ServiceResponse<SportTypeDTO> response = new();
            try
            {
                if (await _sportTypeRepo.SportTypeExistAsync(createSportTypeDTO.Name))
                {
                    response.Message = "Ten Sport już istnieje";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }

                var newSportType = _mapper.Map<SportType>(createSportTypeDTO);

                if (!await _sportTypeRepo.CreateSportTypeAsync(newSportType))
                {
                    response.Message = "Błąd przy dodawaniu sportu";
                    response.Success = false;
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<SportTypeDTO>(newSportType);
                response.Message = "Created";

                List<SportTypeDTO> sportsTypeDTOList = new List<SportTypeDTO>();
                if (_memoryCache.TryGetValue("AllSportTypes", out sportsTypeDTOList))
                {
                    if(sportsTypeDTOList != null)
                    {
                        sportsTypeDTOList.Add(response.Data);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllSportTypes", sportsTypeDTOList, cacheOptions);
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

        public async Task<ServiceResponse<string>> DeleteSportType(int Id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                SportType _existingSportType = await _sportTypeRepo.GetSportTypeByIdAsync(Id);

                if (_existingSportType == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono sportu do usnięcia";
                    _response.Data = null;
                    return _response;
                }

                if (!await _sportTypeRepo.DeleteSportTypeAsync(_existingSportType))
                {
                    _response.Success = false;
                    _response.Message = "Nie udało się usunąć pozycji";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "Deleted";
                List<SportTypeDTO> sportsTypeDTOList = new List<SportTypeDTO>();
                if (_memoryCache.TryGetValue("AllSportTypes", out sportsTypeDTOList))
                {
                    if (sportsTypeDTOList != null)
                    {
                        sportsTypeDTOList.RemoveAll(x => x.SportTypeID == Id);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllSportTypes", sportsTypeDTOList, cacheOptions);
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

        public async Task<ServiceResponse<List<SportTypeDTO>>> GetAllSportTypesAsync()
        {
            ServiceResponse<List<SportTypeDTO>> _response = new();
            try
            {
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetSize(1024);

                List<SportTypeDTO> sportsTypeDTOList = new List<SportTypeDTO>();
                if (!_memoryCache.TryGetValue("AllSportTypes", out sportsTypeDTOList))
                {
                    ICollection<SportType> Sports = await _sportTypeRepo.GetAllSportTypesAsync();
                    sportsTypeDTOList = new List<SportTypeDTO>();
                    foreach (var item in Sports)
                    {
                        sportsTypeDTOList.Add(_mapper.Map<SportTypeDTO>(item));
                    }
                    _memoryCache.Set("AllSportTypes", sportsTypeDTOList, cacheOptions);
                }

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = sportsTypeDTOList;

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

        public async Task<ServiceResponse<SportTypeDTO>> GetByIdAsync(int Id)
        {
            ServiceResponse<SportTypeDTO> _response = new();
            try
            {
                SportType sportType = await _sportTypeRepo.GetSportTypeByIdAsync(Id);

                if (sportType == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono sportu";
                    return _response;
                }

                SportTypeDTO sportTypeDTO = _mapper.Map<SportTypeDTO>(sportType);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = sportTypeDTO;

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

        public async Task<ServiceResponse<SportTypeDTO>> UpdateSportTypeAsync(UpdateSportTypeDTO updateSportTypeDTO)
        {
            ServiceResponse<SportTypeDTO> _response = new();

            try
            {
 
                SportType _existingSportType = await _sportTypeRepo.GetSportTypeByIdAsync(updateSportTypeDTO.SportTypeID);

                if (_existingSportType == null)
                {
                    _response.Success = false;
                    _response.Message = "Nie znaleziono sportu do zaktualizowania";
                    _response.Data = null;
                    return _response;

                }
                _mapper.Map(updateSportTypeDTO, _existingSportType);

                if (!await _sportTypeRepo.UpdateSportTypeAsync(_existingSportType))
                {
                    _response.Success = false;
                    _response.Message = "Błąd przy aktualizacji sportu";
                    _response.Data = null;
                    return _response;
                }

                SportTypeDTO sportTypeDTO = _mapper.Map<SportTypeDTO>(_existingSportType);
                _response.Success = true;
                _response.Message = "Updated";
                _response.Data = sportTypeDTO;

                List<SportTypeDTO> sportsTypeDTOList = new List<SportTypeDTO>();
                if (_memoryCache.TryGetValue("AllSportTypes", out sportsTypeDTOList))
                {
                    if (sportsTypeDTOList != null)
                    {
                        SportTypeDTO oldSportInCache = sportsTypeDTOList.Find(x => x.SportTypeID == updateSportTypeDTO.SportTypeID);
                        foreach (PropertyInfo property in typeof(SportTypeDTO).GetProperties().Where(p => p.CanWrite))
                        {
                            property.SetValue(oldSportInCache, property.GetValue(sportTypeDTO, null), null);
                        }
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetSize(1024);
                        _memoryCache.Set("AllSportTypes", sportsTypeDTOList, cacheOptions);
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
