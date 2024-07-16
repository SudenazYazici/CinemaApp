using AutoMapper;
using CinemaApp.Dto;
using CinemaApp.Interface;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMovies()
        {
            var movies = _mapper.Map<List<MovieDto>>(_movieRepository.GetMovies());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(movies);
        }

        [HttpGet("by-name/{name}")]
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(400)]
        public IActionResult GetMovie(string name)
        {
            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(name));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(movie);
        }

        [HttpGet("by-id/{id}")]
        [ProducesResponseType(200, Type = typeof(Movie))]
        [ProducesResponseType(400)]
        public IActionResult GetMovie(int id)
        {
            if (!_movieRepository.MovieExists(id))
            {
                return NotFound();
            }
            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateMovie([FromBody] MovieDto movieCreate)
        {
            if (movieCreate == null)
            {
                return BadRequest(ModelState);
            }

            var movie = _movieRepository.GetMovies()
                .Where(m => m.Id == movieCreate.Id).FirstOrDefault();

            if (movie != null)
            {
                ModelState.AddModelError("", "Movie already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieMap = _mapper.Map<Movie>(movieCreate);
            if (!_movieRepository.CreateMovie(movieMap))
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
        public IActionResult UpdateMovie(int id, [FromBody] MovieDto movieUpdate)
        {
            if (movieUpdate == null || id != movieUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_movieRepository.MovieExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var movieMap = _mapper.Map<Movie>(movieUpdate);

            if (!_movieRepository.UpdateMovie(movieMap))
            {
                ModelState.AddModelError("", "Something went wrong updating movie");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMovie(int id)
        {

            if (!_movieRepository.MovieExists(id))
            {
                return NotFound();
            }

            var movie = _movieRepository.GetMovie(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", "Something went wrong deleting movie");
            }

            return NoContent();
        }
    }
}
