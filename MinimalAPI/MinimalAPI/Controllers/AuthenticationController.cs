using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPI.DB.IRepository;
using MinimalAPI.DB.Repository;
using MinimalAPI.DTO;
using MinimalAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthenticationController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            if (authenticationRequestBody == null || string.IsNullOrEmpty(authenticationRequestBody.Username) || string.IsNullOrEmpty(authenticationRequestBody.Password))
            {
                return BadRequest(new { message = "Invalid input. Username and password are both required." });
            }
            var user = _userRepository.GetUserByUsernameAndPasswordAsync(
                authenticationRequestBody.Username,
                authenticationRequestBody.Password);
            if(user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", authenticationRequestBody.Username));
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);
            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);
        }
        [HttpPost("signup")]
        public async Task<ActionResult> Signup(AuthenticationRequestBody authenticationRequestBody)
        {
            if (authenticationRequestBody == null || string.IsNullOrEmpty(authenticationRequestBody.Username) || string.IsNullOrEmpty(authenticationRequestBody.Password))
            {
                return BadRequest(new { message = "Invalid input. Username and password are both required." });
            }
            bool isUsernameTaken = await _userRepository.UserExistsByNameAsync(authenticationRequestBody.Username);
            if (isUsernameTaken)
            {
                return Conflict(new { message = "Username is already taken." });
            }
            var newUser = new User(authenticationRequestBody.Username,
                authenticationRequestBody.Password);
            _userRepository.AddAsync(newUser);
            return Ok();
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            return Ok(user);
        }
    }
}
