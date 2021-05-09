using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EducationalConsultAPI.Controllers {
    [ApiController]
    [Route("/api/login")]
    public class LoginController : ControllerBase{

        public LoginController(
            IRepository<User> repository, IMapper mapper, ISecurityService securityService
        ) {
            _userRepository = repository;
            _mapper = mapper;
            _security = securityService;
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
    }
}
