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
        private readonly IMapper _mapper;

        public TicketController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
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
