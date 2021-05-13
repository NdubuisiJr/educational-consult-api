using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using static System.IO.File;

namespace EducationalConsultAPI.Controllers {
    [ApiController]
    [Route("/api/login")]
    public class LoginController : ControllerBase{

        public LoginController(
            IRepository<User> repository, IMapper mapper, ISecurityService securityService,
            ICommunication communication
        ) {
            _userRepository = repository;
            _mapper = mapper;
            _security = securityService;
            _communication = communication;
        }

        /// <summary>
        /// Invalidates the user's password
        /// </summary>
        /// <param name="email">The user's email for authentication</param>
        /// <returns></returns>
        [HttpPost("forgotpassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ForgotPassword([FromQuery]string email) {
            try {
                if (email is null)
                    return BadRequest(new Response<object>(400, "The email address is invalid"));

                var user = _userRepository.GetAll()
                        .Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower())
                        .FirstOrDefault();

                if (user is null)
                    return NotFound(new Response<object>(400, "No such user"));

                var verification = new Random().Next(1000, 20000);
                var newPassword = $"educational_consult_{verification}";

                var emailPath = Helpers.GetFilePath("/password.html");
                var html = ReadAllText(emailPath);
                html = html.Replace("{{action_url}}", newPassword);

                var result = _communication.SendEmail(user.Email, "Password Reset", html);
                if (!result)
                    return UnprocessableEntity(new Response<object>(422, "Unable to send email"));

                var passData = _security.HashPassword(newPassword);
                user.PasswordSalt = passData.Item1;
                user.PasswordHash = passData.Item2;

                _userRepository.Update(user);
                _userRepository.SaveChanges();
                return Ok(new Response<object>(200, "password reset completed"));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        /// <summary>
        /// Performs user authentication and returns an authentication token
        /// </summary>
        /// <param name="login">The login object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Response<UserResponse>> Login([FromBody]UserLogin login) {
            try {
                if (login is null)
                    return BadRequest(new Response<object>(400, "Invalid data"));

                var user = _userRepository.GetAll()
                        .FirstOrDefault(x => x.Email.Trim() == login.Email.Trim());

                if (user is null)
                    return BadRequest(new Response<object>(400, "Invalid email or password"));

                if(!_security.VerifyPassword(login.Password, user.PasswordHash, user.PasswordSalt))
                    return BadRequest(new Response<object>(400, "Invalid email or password"));

                if (!user.IsVerified)
                    return BadRequest(new Response<object>(400, "Please verify your email"));

                var token = _security.GenerateJwtToken(user);
                var userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Token = token;

                return Ok(new Response<UserResponse>(200, "login successfully", userResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ISecurityService _security;
        private readonly ICommunication _communication;
    }
}
