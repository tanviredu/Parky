﻿using System;
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
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark( int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if(obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<NationalParkDto>(obj);

            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if( _npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }


            var obj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = obj.Id}, obj);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
             if( nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var obj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
