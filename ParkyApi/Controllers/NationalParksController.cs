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
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
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

        /// <summary>
        /// Get indivisual  national park
        /// </summary>
        /// <param name="nationalParkId"> The id of a national park</param>
        /// <returns></returns>
        
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
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

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var obj = _npRepo.GetNationalPark(nationalParkId);

            if (!_npRepo.DeleteNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when Deleting the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
