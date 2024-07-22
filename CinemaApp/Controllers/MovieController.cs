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
    public class MovieController(IMovieRepository movieRepository, IMapper mapper, ICinemaMovieRepository cinemaMovieRepository)
        : Controller
    {
        //private readonly ICinemaRepository _cinemaRepository = cinemaRepository;

        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        //public IActionResult GetMovies()
        //{
        //    var movies = movieRepository.GetMovies();
        //    var cinemaMovies = cinemaRepository.GetCinemaHalls();

        //    var movieList = movies.Select(movie => new MovieDto
        //        { CinemaId = cinemaMovies.First(cinemaMovie => cinemaMovie.MovieId == movie.Id).CinemaId, Details = movie.Details, Id = movie.Id, Name = movie.Name}).ToList();
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return Ok(movies);
        //}

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMovies()
        {
            var movies = mapper.Map<List<MovieDto>>(movieRepository.GetMovies());

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
            var movie = mapper.Map<MovieDto>(movieRepository.GetMovie(name));
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
            if (!movieRepository.MovieExists(id))
            {
                return NotFound();
            }
            var movie = mapper.Map<MovieDto>(movieRepository.GetMovie(id));
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

            var movie = movieRepository.GetMovies()
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

            //var cinema = _cinemaRepository.GetCinema(movieCreate.CinemaId);

            //if (cinema == null)
            //{
            //    ModelState.AddModelError("", "Cinema does not exist");
            //    return NotFound(ModelState);
            //}

            var movieMap = mapper.Map<Movie>(movieCreate);
            //var cinemaMovie = new CinemaMovie
            //{
            //    cinema = cinema,
            //    movie = movieMap
            //};
            //cinema.CinemaMovies.Add(cinemaMovie);
            if (!movieRepository.CreateMovie(movieMap))
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
            if (!movieRepository.MovieExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var movieMap = mapper.Map<Movie>(movieUpdate);

            if (!movieRepository.UpdateMovie(movieMap))
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

            if (!movieRepository.MovieExists(id))
            {
                return NotFound();
            }

            var movie = movieRepository.GetMovie(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", "Something went wrong deleting movie");
            }

            return NoContent();
        }

        [HttpGet("{movieId}/cinemas")]
        public IActionResult GetCinemasOfMovie(int movieId)
        {
            var cinemas = cinemaMovieRepository.GetCinemasOfMovie(movieId);
            return Ok(cinemas);
        }

        [HttpPost("{movieId}/post-to-cinema/{cinemaId}")]
        public IActionResult AddCinemaMovie(int movieId, int cinemaId)
        {
            cinemaMovieRepository.AddCinemaMovie(movieId, cinemaId);
            return NoContent();
        }
    }
}
