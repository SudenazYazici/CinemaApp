using AutoMapper;
using CinemaApp.Dto;
using CinemaApp.Interface;
using CinemaApp.Models;
using CinemaApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IMapper _mapper;

        public CinemaController(ICinemaRepository cinemaRepository, IMapper mapper)
        {
            _cinemaRepository = cinemaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Cinema>))]
        public IActionResult GetCinemas()
        {
            var cinemas = _mapper.Map<List<CinemaDto>>(_cinemaRepository.GetCinemas());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Cinema))]
        [ProducesResponseType(400)]
        public IActionResult GetCinema(int id)
        {
            if (!_cinemaRepository.CinemaExists(id))
            {
                return NotFound();
            }
            var cinema = _mapper.Map<CinemaDto>(_cinemaRepository.GetCinema(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cinema);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCinema([FromBody] CinemaDto cinemaCreate)
        {
            if (cinemaCreate == null)
            {
                return BadRequest(ModelState);
            }

            var cinema = _cinemaRepository.GetCinemas()
                .Where(c => c.Id == cinemaCreate.Id).FirstOrDefault();

            if (cinema != null)
            {
                ModelState.AddModelError("", "Cinema already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cinemaMap = _mapper.Map<Cinema>(cinemaCreate);
            if (!_cinemaRepository.CreateCinema(cinemaMap))
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
        public IActionResult UpdateCinema(int id, [FromBody] CinemaDto cinemaUpdate)
        {
            if (cinemaUpdate == null || id != cinemaUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_cinemaRepository.CinemaExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var cinemaMap = _mapper.Map<Cinema>(cinemaUpdate);

            if (!_cinemaRepository.UpdateCinema(cinemaMap))
            {
                ModelState.AddModelError("", "Something went wrong updating cinema");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCinema(int id) {
            
            if(!_cinemaRepository.CinemaExists(id))
            {
                return NotFound();
            }

            var cinema = _cinemaRepository.GetCinema(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!_cinemaRepository.DeleteCinema(cinema))
            {
                ModelState.AddModelError("", "Something went wrong deleting cinema");
            }

            return NoContent();
        }

        [HttpGet("{cinemaId}/cinemaHalls")]
        public IActionResult GetCinemaHalls(int cinemaId)
        {
            var cinemaHalls = _cinemaRepository.GetCinemaHalls(cinemaId)
                .Where(ch => ch.CinemaId == cinemaId)
                .OrderBy(ch => ch.Id)
                .ToList();
            return Ok(cinemaHalls);
        }

    }
}
