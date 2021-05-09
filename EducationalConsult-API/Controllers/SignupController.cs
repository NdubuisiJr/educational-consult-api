using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.IO.File;
using static EducationalConsultAPI.Utils.Constants;
using System;

namespace EducationalConsultAPI.Controllers {

    [ApiController]
    [Route("/api/signup")]
    public class SignupController : ControllerBase{

        public SignupController(
            IMapper mapper, IRepository<User> userRepository, ISecurityService securityService,
            ICommunication communication) {
            _mapper = mapper;
            _userRepository = userRepository;
            _securityService = securityService;
            _communication = communication;
        }

        /// <summary>
        /// Verifies a user's email
        /// </summary>
        /// <param name="verification">The verification code sent to the user's email</param>
        /// <param name="email">The user's email address</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Verify([FromQuery]string verification, [FromQuery]string email) {
            try {
                if (verification is null || email is null)
                    return BadRequest(new Response<object>(400, "empty request"));

                var user = _userRepository.GetAll()
                        .FirstOrDefault(x => x.VerificationCode == int.Parse(verification) && x.Email == email);
                if (user is null)
                    return BadRequest(new Response<object>(400, "empty request"));

                user.IsVerified = true;

                _userRepository.Update(user);
                _userRepository.SaveChanges();

                return Redirect($"{FRONTEND_BASE_URL}/login?id={user.Id}");
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        /// <summary>
        /// Register's a new user
        /// </summary>
        /// <param name="userRegistration">Manages the entire required field</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Response<UserResponse>> Signup([FromBody] UserRegistration userRegistration) {
            try {
                if (userRegistration is null)
                    return BadRequest(new Response<UserResponse>(400, "The body is null"));

                var user = _mapper.Map<User>(userRegistration);

                var existing = _userRepository.GetAll()
                    .FirstOrDefault(x => x.Email.Trim().ToLower() == user.Email.Trim().ToLower() || x.Phone.Trim() == user.Phone.Trim());

                if (existing is { })
                    return BadRequest(new Response<UserResponse>(400, "A user with the same email or phone already exists"));

                var verification = new Random().Next(1000, 20000);
                var passData = _securityService.HashPassword(userRegistration.Password);
                user.PasswordSalt = passData.Item1;
                user.PasswordHash = passData.Item2;
                user.VerificationCode = verification;

                var url = $"{BACKEND_BASE_URL}/api/signup?verification={verification}&email={user.Email}";
                var emailPath = Helpers.GetFilePath("/email.html");
                var html = ReadAllText(emailPath);
                html = html.Replace("{{action_url}}", url);
                html = html.Replace("{{action_url}}", url);
                html = html.Replace("{{action_url}}", url);

                var result = _communication.SendEmail(user.Email, "Verify Email", html);
                if (!result)
                    return BadRequest(new Response<UserResponse>(400, "Unable to send verification email"));

                _userRepository.Add(user);
                _userRepository.SaveChanges();

                var userResponse = _mapper.Map<UserResponse>(user);

                return Created("/api/login", new Response<UserResponse>(201, "Signup successful", userResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly ISecurityService _securityService;
        private readonly ICommunication _communication;
    }
}
