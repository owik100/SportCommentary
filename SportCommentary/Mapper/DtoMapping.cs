using AutoMapper;
using SportCommentaryDataAccess.DTO;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SportCommentary.Mapper
{
    public class DtoMapping: Profile
    {
        public DtoMapping()
        {
            CreateMap<CreateSportTypeDTO, SportType>();
            CreateMap<SportType, SportTypeDTO>();
            CreateMap<UpdateSportTypeDTO, SportType>();
            CreateMap<SportTypeDTO, UpdateSportTypeDTO>();
        }
    }
}
