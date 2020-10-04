using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national park
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(200, Type =typeof(List<NationalParkDto>))]
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
