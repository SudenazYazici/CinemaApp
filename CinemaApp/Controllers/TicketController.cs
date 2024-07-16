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
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly ICinemaHallRepository _cinemaHallRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public TicketController(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            ICinemaRepository cinemaRepository,
            ICinemaHallRepository cinemaHallRepository,
            ISeatRepository seatRepository,
            IMovieRepository movieRepository,
            IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _cinemaRepository = cinemaRepository;
            _cinemaHallRepository = cinemaHallRepository;
            _seatRepository = seatRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Ticket>))]
        public IActionResult GetTickets()
        {
            var tickets = _mapper.Map<List<TicketDto>>(_ticketRepository.GetTickets());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Ticket))]
        [ProducesResponseType(400)]
        public IActionResult GetTicket(int id)
        {
            if (!_ticketRepository.TicketExists(id))
            {
                return NotFound();
            }
            var ticket = _mapper.Map<TicketDto>(_ticketRepository.GetTicket(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(ticket);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTicket([FromBody] TicketDto ticketCreate)
        {
            if (ticketCreate == null)
            {
                return BadRequest(ModelState);
            }

            var ticket = _ticketRepository.GetTickets()
                .Where(t => t.Id == ticketCreate.Id).FirstOrDefault();

            if (ticket != null)
            {
                ModelState.AddModelError("", "Ticket already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketMap = _mapper.Map<Ticket>(ticketCreate);

            var user = _userRepository.GetUser(ticketCreate.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User does not exist");
                return StatusCode(404, ModelState);
            }

            //var cinema = _cinemaRepository.GetCinema(ticketCreate.CinemaId);
            //if (cinema == null)
            //{
            //    ModelState.AddModelError("", "Cinema does not exist");
            //    return StatusCode(404, ModelState);
            //}

            //var cinemaHall = _cinemaHallRepository.GetCinemaHall(ticketCreate.CinemaHallId);
            //if (cinemaHall == null)
            //{
            //    ModelState.AddModelError("", "Cinema Hall does not exist");
            //    return StatusCode(404, ModelState);
            //}

            //var seat = _seatRepository.GetSeat(ticketCreate.SeatId);
            //if (seat == null)
            //{
            //    ModelState.AddModelError("", "Seat does not exist");
            //    return StatusCode(404, ModelState);
            //}

            //var movie = _movieRepository.GetMovie(ticketCreate.MovieName);
            //if (movie == null)
            //{
            //    ModelState.AddModelError("", "Movie does not exist");
            //    return StatusCode(404, ModelState);
            //}

            //ticketMap.User = user;
            //ticketMap.Cinema = cinema;
            //ticketMap.CinemaHall = cinemaHall;
            //ticketMap.Seat = seat;
            //ticketMap.MovieName = movie.Name;  
            user.Tickets.Add(ticketMap);

            if (!_ticketRepository.CreateTicket(ticketMap))
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
        public IActionResult UpdateTicket(int id, [FromBody] TicketDto ticketUpdate)
        {
            if (ticketUpdate == null || id != ticketUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_ticketRepository.TicketExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ticketMap = _mapper.Map<Ticket>(ticketUpdate);

            if (!_ticketRepository.UpdateTicket(ticketMap))
            {
                ModelState.AddModelError("", "Something went wrong updating ticket");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTicket(int id) { 
            
            if(!_ticketRepository.TicketExists(id)) return NotFound();

            var ticket = _ticketRepository.GetTicket(id);

            if (!ModelState.IsValid) return BadRequest();

            if(!_ticketRepository.DeleteTicket(ticket)) {
                ModelState.AddModelError("", "Something went wrong deleting ticket");
            }

            return NoContent();

        }
    }
}
