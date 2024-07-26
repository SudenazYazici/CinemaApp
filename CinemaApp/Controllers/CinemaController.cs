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
    public class CinemaController(
        ICinemaRepository cinemaRepository,
        IMapper mapper,
        ICinemaMovieRepository cinemaMovieRepository)
        : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Cinema>))]
        [AllowAnonymous]
        public IActionResult GetCinemas()
        {
            var cinemas = mapper.Map<List<CinemaDto>>(cinemaRepository.GetCinemas());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Cinema))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public IActionResult GetCinema(int id)
        {
            if (!cinemaRepository.CinemaExists(id))
            {
                return NotFound();
            }
            var cinema = mapper.Map<CinemaDto>(cinemaRepository.GetCinema(id));
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

            var cinema = cinemaRepository.GetCinemas()
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

            var cinemaMap = mapper.Map<Cinema>(cinemaCreate);
            if (!cinemaRepository.CreateCinema(cinemaMap))
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
            if (!cinemaRepository.CinemaExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var cinemaMap = mapper.Map<Cinema>(cinemaUpdate);

            if (!cinemaRepository.UpdateCinema(cinemaMap))
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
            
            if(!cinemaRepository.CinemaExists(id))
            {
                return NotFound();
            }

            var cinema = cinemaRepository.GetCinema(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!cinemaRepository.DeleteCinema(cinema))
            {
                ModelState.AddModelError("", "Something went wrong deleting cinema");
            }

            return NoContent();
        }

        [HttpGet("{cinemaId}/cinemaHalls")]
        [AllowAnonymous]
        public IActionResult GetCinemaHalls(int cinemaId)
        {
            var cinemaHalls = cinemaRepository.GetCinemaHalls(cinemaId)
                .Where(ch => ch.CinemaId == cinemaId)
                .OrderBy(ch => ch.Id)
                .ToList();
            return Ok(cinemaHalls);
        }

        [HttpGet("{cinemaId}/movies")]
        [AllowAnonymous]
        public IActionResult GetMoviesOfCinema(int cinemaId)
        {
            var movies = cinemaMovieRepository.GetMoviesOfCinema(cinemaId);
                
            return Ok(movies);
        }


    }
}
