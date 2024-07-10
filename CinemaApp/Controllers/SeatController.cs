﻿using AutoMapper;
using CinemaApp.Dto;
using CinemaApp.Interface;
using CinemaApp.Models;
using CinemaApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : Controller
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;

        public SeatController(ISeatRepository seatRepository, IMapper mapper)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Seat>))]
        public IActionResult GetSeats()
        {
            var seats = _mapper.Map<List<SeatDto>>(_seatRepository.GetSeats());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(seats);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Seat))]
        [ProducesResponseType(400)]
        public IActionResult GetSeat(int id)
        {
            if (!_seatRepository.SeatExists(id))
            {
                return NotFound();
            }
            var user = _mapper.Map<SeatDto>(_seatRepository.GetSeat(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSeat([FromBody] SeatDto seatCreate)
        {
            if (seatCreate == null)
            {
                return BadRequest(ModelState);
            }

            var seat = _seatRepository.GetSeats()
                .Where(s => s.Id == seatCreate.Id).FirstOrDefault();

            if (seat != null)
            {
                ModelState.AddModelError("", "Seat already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seatMap = _mapper.Map<Seat>(seatCreate);
            if (!_seatRepository.CreateSeat(seatMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSeat(int id, [FromBody] SeatDto seatUpdate)
        {
            if (seatUpdate == null || id != seatUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_seatRepository.SeatExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var seatMap = _mapper.Map<Seat>(seatUpdate);

            if (!_seatRepository.UpdateSeat(seatMap))
            {
                ModelState.AddModelError("", "Something went wrong updating seat");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSeat(int id) { 
            
            if(!_seatRepository.SeatExists(id))
            {
                return NotFound();
            }

            var seat = _seatRepository.GetSeat(id);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!_seatRepository.DeleteSeat(seat))
            {
                ModelState.AddModelError("", "Something went wrong deleting seat");
            }

            return NoContent();
        }
    }
}
