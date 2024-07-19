using AutoMapper;
using Azure.Core;
using CinemaApp.Dto;
using CinemaApp.Interface;
using CinemaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public UserController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id) { 
            if(!_userRepository.UserExists(id))
            {
                return NotFound();
            }
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpGet("{id}/tickets")] 
        [ProducesResponseType(200, Type = typeof(Ticket))]
        [ProducesResponseType(400)]
        public IActionResult GetTicketsByUser(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            var tickets = _mapper.Map<List<TicketDto>>(
                _userRepository.GetTicketsByUser(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tickets);
        }

        /*[HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if(userCreate == null) {
                return BadRequest(ModelState); 
            }

            var user = _userRepository.GetUsers()
                .Where(u =>  u.Id == userCreate.Id).FirstOrDefault();

            if (user != null) {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userMap = _mapper.Map<User>(userCreate);
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }*/

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userUpdate) {
            if (userUpdate == null || id != userUpdate.Id) { 
                return BadRequest(ModelState);
            }
            if (!_userRepository.UserExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdate);

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int id) { 

            if(!_userRepository.UserExists(id)) {
                return NotFound(); 
            }

            var user = _userRepository.GetUser(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.DeleteUser(user)) {
                ModelState.AddModelError("", "Something went wrong deleting user");
            }

            return NoContent();

        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request) {

            if (!ModelState.IsValid) {
                return BadRequest();
            }
     
            if (_userRepository.UserExists(request.Name))
            {
                return BadRequest("This user has already registered.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            //string confirmPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.ConfirmPassword);

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
                BirthDate = request.BirthDate,
                Role = request.Role ?? "user"
            };
            
            //user.ConfirmPassword = confirmPasswordHash;

            _userRepository.CreateUser(newUser);
            return Ok(newUser);
        }

        //[HttpPost("login")]
        //[ProducesResponseType(404)]
        //public IActionResult logIn([FromBody]string email, [FromBody] string password)
        //{
        //    bool userExists = false;
        //    var users = _userRepository.GetUsers();

        //    for (int i = 0; i < users.Count; i++)
        //    {
        //        var user = _userRepository.GetUsers().ElementAt(i);
        //        if (user.Email == email && BCrypt.Net.BCrypt.Verify(password, user.Password))
        //        {
        //            userExists = true;
        //            return Ok(user);
        //        }
        //    }

        //    if (!userExists)
        //    {
        //        return BadRequest("User not found.");
        //    }

        //    string token = CreateToken(user);
        //    return Ok(token);

        //}

        [HttpPost("login")]
        public ActionResult<User> Login(LoginDto request)
        {
            var user = _userRepository.GetUserByEmail(request.Email);

            if (user == null )
            {
                return BadRequest("Account not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            var loggedInUser = new LoggedInDto();
            loggedInUser.Id = user.Id;
            loggedInUser.Name = user.Name;
            loggedInUser.Email = user.Email;
            loggedInUser.BirthDate = user.BirthDate;
            loggedInUser.Role = user.Role;
            loggedInUser.Token = token;

            return Ok(loggedInUser);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
