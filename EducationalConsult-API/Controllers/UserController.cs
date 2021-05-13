using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EducationalConsultAPI.Controllers {
    [Authorize]
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase {

        public UserController(IRepository<User> repository, ISecurityService securityService) {
            _userRepository = repository;
            _securityService = securityService;
        }

        /// <summary>
        /// Changes a user's password
        /// </summary>
        /// <param name="userId">The user's unique Id</param>
        /// <param name="password">The new password</param>
        /// <returns></returns>
        [HttpPatch("{userId}/changepassword")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ChangePassword(Guid userId, [FromQuery]string password) {
            try {
                if (password is null || userId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "auth failed"));

                var user = _userRepository.Get(userId);

                if (user is null)
                    return BadRequest(new Response<object>(400, "auth failed"));

                var passData = _securityService.HashPassword(password);
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

        private readonly IRepository<User> _userRepository;
        private readonly ISecurityService _securityService;
    }
}
