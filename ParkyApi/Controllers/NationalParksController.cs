using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();

            var objDtoList = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDtoList.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDtoList);
        }
    }
}
