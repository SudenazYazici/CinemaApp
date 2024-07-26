using AutoMapper;
using CinemaApp.Dto;
using CinemaApp.Interface;
using CinemaApp.Models;
using CinemaApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class CinemaHallController : Controller
    {
        private readonly ICinemaHallRepository _cinemaHallRepository;
        private readonly IMapper _mapper;
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaHallController(ICinemaHallRepository cinemaHallRepository, IMapper mapper, ICinemaRepository cinemaRepository)
        {
            _cinemaHallRepository = cinemaHallRepository;
            _mapper = mapper;
            _cinemaRepository = cinemaRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CinemaHall>))]
        [AllowAnonymous]
        public IActionResult GetCinemaHalls()
        {
            var cinemaHalls = _mapper.Map<List<CinemaHallDto>>(_cinemaHallRepository.GetCinemaHalls());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(cinemaHalls);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CinemaHall))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public IActionResult GetCinemaHall(int id)
        {
            if (!_cinemaHallRepository.CinemaHallExists(id))
            {
                return NotFound();
            }
            var cinemaHall = _mapper.Map<CinemaHallDto>(_cinemaHallRepository.GetCinemaHall(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cinemaHall);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCinemaHall([FromBody] CinemaHallDto cinemaHallCreate)
        {
            if (cinemaHallCreate == null)
            {
                return BadRequest(ModelState);
            }

            var cinemaHall = _cinemaHallRepository.GetCinemaHalls()
                .Where(c => c.Id == cinemaHallCreate.Id).FirstOrDefault();

            if (cinemaHall != null)
            {
                ModelState.AddModelError("", "Cinema hall already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cinema = _cinemaRepository.GetCinema(cinemaHallCreate.CinemaId);

            if (cinema == null)
            {
                ModelState.AddModelError("", "Cinema does not exist");
                return NotFound(ModelState);
            }

            var cinemaHallMap = _mapper.Map<CinemaHall>(cinemaHallCreate);
            cinema.CinemaHalls.Add(cinemaHallMap);
            if (!_cinemaHallRepository.CreateCinemaHall(cinemaHallMap))
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
        public IActionResult UpdateCinemaHall(int id, [FromBody] CinemaHallDto cinemaHallUpdate)
        {
            if (cinemaHallUpdate == null || id != cinemaHallUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_cinemaHallRepository.CinemaHallExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var cinemaHallMap = _mapper.Map<CinemaHall>(cinemaHallUpdate);

            if (!_cinemaHallRepository.UpdateCinemaHall(cinemaHallMap))
            {
                ModelState.AddModelError("", "Something went wrong updating cinema hall");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCinemaHall(int id)
        {
            if(!_cinemaHallRepository.CinemaHallExists(id) )
            {
                return NotFound();
            }

            var cinemaHall = _cinemaHallRepository.GetCinemaHall(id);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_cinemaHallRepository.DeleteCinemaHall(cinemaHall))
            {
                ModelState.AddModelError("", "Something went wrong deleting cinema hall");
            }

            return NoContent();
        }

        [HttpGet("{cinemaHallId}/seats")]
        [AllowAnonymous]
        public IActionResult GetSeats(int cinemaHallId)
        {
            var seats = _cinemaHallRepository.GetSeats(cinemaHallId)
                .Where(ch => ch.CinemaHallId == cinemaHallId)
                .OrderBy(ch => ch.Id)
                .ToList();
            return Ok(seats);
        }
    }
}
