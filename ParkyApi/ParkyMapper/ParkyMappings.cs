﻿using AutoMapper;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.ParkyMapper
{
    public class ParkyMappings: Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailUpsertDto>().ReverseMap();
        }
    }
}
