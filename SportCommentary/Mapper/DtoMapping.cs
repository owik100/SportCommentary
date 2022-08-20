using AutoMapper;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SportCommentary.Mapper
{
    public class DtoMapping: Profile
    {
        public DtoMapping()
        {
            CreateMap<SportType, SportTypeDTO>().ReverseMap();
            CreateMap<CreateSportTypeDTO, SportTypeDTO>().ReverseMap();
            CreateMap<UpdateSportTypeDTO, SportTypeDTO>().ReverseMap();
        }
    }
}
