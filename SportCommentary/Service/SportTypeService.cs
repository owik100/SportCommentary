using AutoMapper;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess;
using SportCommentaryDataAccess.DTO;
using SportCommentaryDataAccess.DTO.Commentary;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;

namespace SportCommentary.Service
{
    public class SportTypeService : ISportTypeService
    {
        private readonly ISportTypeRepository _sportTypeRepo;
        private readonly IMapper _mapper;
        public SportTypeService(IMapper mapper, ISportTypeRepository sportTypeRepo)
        {
            _mapper = mapper;
            _sportTypeRepo = sportTypeRepo;
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
                ICollection<SportType> Sports = await _sportTypeRepo.GetAllSportTypesAsync();

                List<SportTypeDTO> sportsTypeDTOList = new List<SportTypeDTO>();

                foreach (var item in Sports)
                {
                    sportsTypeDTOList.Add(_mapper.Map<SportTypeDTO>(item));
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
