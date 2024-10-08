﻿using AutoMapper;
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
    public class SessionController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly ICinemaHallRepository _cinemaHallRepository;
        private readonly ICinemaRepository _cinemaRepository;

        public SessionController(ISessionRepository sessionRepository, IMapper mapper, IMovieRepository movieRepository, ICinemaHallRepository cinemaHallRepository, ICinemaRepository cinemaRepository)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _movieRepository = movieRepository;
            _cinemaHallRepository = cinemaHallRepository;
            _cinemaRepository = cinemaRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Session>))]
        [AllowAnonymous]
        public IActionResult GetSessions()
        {
            var sessions = _mapper.Map<List<SessionDto>>(_sessionRepository.GetSessions());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Session))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public IActionResult GetSession(int id)
        {
            if (!_sessionRepository.SessionExists(id))
            {
                return NotFound();
            }
            var session = _mapper.Map<SessionDto>(_sessionRepository.GetSession(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(session);
        }

        [HttpGet("get-sessions/{cinemaId}/{movieId}/{cinemaHallId}")]
        [AllowAnonymous]
        public IActionResult GetSessions(int cinemaId, int movieId, int cinemaHallId)
        {
            var sessions = _sessionRepository.GetSessions(cinemaId, movieId, cinemaHallId);
            if (sessions == null || !sessions.Any())
            {
                return NotFound();
            }

            return Ok(sessions);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSession([FromBody] SessionDto sessionCreate)
        {
            if (sessionCreate == null)
            {
                return BadRequest(ModelState);
            }

            var session = _sessionRepository.GetSessions()
                .Where(s => s.Id == sessionCreate.Id).FirstOrDefault();

            if (session != null)
            {
                ModelState.AddModelError("", "Session already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cinema = _cinemaRepository.GetCinema(sessionCreate.CinemaId);

            if (cinema == null)
            {
                ModelState.AddModelError("", "Cinema does not exist");
                return NotFound(ModelState);
            }

            var movie = _movieRepository.GetMovie(sessionCreate.MovieId);

            if (movie == null)
            {
                ModelState.AddModelError("", "Movie does not exist");
                return NotFound(ModelState);
            }
            var cinemaHall = _cinemaHallRepository.GetCinemaHall(sessionCreate.CinemaHallId);

            if (cinemaHall == null)
            {
                ModelState.AddModelError("", "Cinema hall does not exist");
                return NotFound(ModelState);
            }

            var sessionMap = _mapper.Map<Session>(sessionCreate);
            cinema.Sessions.Add(sessionMap);
            movie.Sessions.Add(sessionMap);
            cinemaHall.Sessions.Add(sessionMap);
            if (!_sessionRepository.CreateSession(sessionMap))
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
        public IActionResult UpdateSession(int id, [FromBody] SessionDto sessionUpdate)
        {
            if (sessionUpdate == null || id != sessionUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_sessionRepository.SessionExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var sessionMap = _mapper.Map<Session>(sessionUpdate);

            if (!_sessionRepository.UpdateSession(sessionMap))
            {
                ModelState.AddModelError("", "Something went wrong updating session");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSession(int id)
        {

            if (!_sessionRepository.SessionExists(id))
            {
                return NotFound();
            }

            var session = _sessionRepository.GetSession(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sessionRepository.DeleteSession(session))
            {
                ModelState.AddModelError("", "Something went wrong deleting session");
            }

            return NoContent();
        }

        //[HttpGet("get-start-time/{movieId}/{cinemaHallId}")]
        //public IActionResult GetSessionsStartTime(int movieId, int cinemaHallId)
        //{
        //    var starts = _sessionRepository.GetSessionsStartTime(movieId, cinemaHallId);
        //    return Ok(starts);
        //}

        [HttpGet("get-session/{cinemaId}/{movieId}/{cinemaHallId}/{startTime}")]
        [AllowAnonymous]
        public IActionResult GetSession(int cinemaId, int movieId, int cinemaHallId, DateTime startTime)
        {
            var session = _sessionRepository.GetSession(cinemaId, movieId, cinemaHallId, startTime);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(session);
        }
    }
}
