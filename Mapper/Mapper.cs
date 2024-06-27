﻿namespace cattoapi.Mapper
{
    using AutoMapper;
    using cattoapi.DTOS;
    using cattoapi.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDTO> ();
        }
    }

}