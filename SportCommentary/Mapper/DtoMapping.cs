﻿using AutoMapper;
using SportCommentaryDataAccess.DTO;
using SportCommentaryDataAccess.DTO.Commentary;
using SportCommentaryDataAccess.DTO.Event;
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


            CreateMap<CreateEventDTO, Event>();
            CreateMap<Event, EventDTO>();
            CreateMap<UpdateEventDTO, Event>();
            CreateMap<EventDTO, UpdateEventDTO>();


            CreateMap<CreateCommentaryDTO, Commentary>();
            CreateMap<Commentary, CommentaryDTO>();
            CreateMap<UpdateCommentaryDTO, Commentary>();
            CreateMap<CommentaryDTO, UpdateCommentaryDTO>();
        }
    }
}
