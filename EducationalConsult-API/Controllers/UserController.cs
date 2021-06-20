using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalConsultAPI.Controllers {
    [Authorize]
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase {

        public UserController(
            IRepository<User> repository, ISecurityService securityService, IMapper mapper
            , IJoinRepository<UserGroup> userGroupRepository) {
            _userRepository = repository;
            _securityService = securityService;
            _mapper = mapper;
            _userGroupRepository = userGroupRepository;
        }

        /// <summary>
        /// Update user details
        /// </summary>
        /// <param name="userId">The user's unique Id</param>
        /// <param name="userUpdateDto">update model for user details</param>
        /// <returns></returns>
        [HttpPatch("{userId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateUser(Guid userId, [FromBody] UserUpdateDto userUpdateDto) {
            try {
                if (userUpdateDto is null || userId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "auth failed"));

                var user = _userRepository.Get(userId);

                if (user is null)
                    return BadRequest(new Response<object>(400, "auth failed"));

                user.Phone = userUpdateDto.Phone;
                user.Surname = userUpdateDto.Surname;
                user.OtherNames = userUpdateDto.OtherNames;

                _userRepository.Update(user);
                _userRepository.SaveChanges();
                return Ok(new Response<object>(200, "Update completed successfully"));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        /// <summary>
        /// Retrieves a user with a given Id
        /// </summary>
        /// <param name="userId">The user's unique Id</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Response<UserResponse>> GetUser(Guid userId) {
            try {
                if (userId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "Invalid Id"));

                var user = _userRepository.Get(userId);
                if (user is null)
                    return NotFound(new Response<object>(404, "User not found"));

                var userResponse = _mapper.Map<UserResponse>(user);
                var schools = _userGroupRepository.GetGroups(userId)
                               .OfType<Group>()
                               .Select(x => x.School);

                var schoolResponses = _mapper.Map<IList<SchoolResponse>>(schools);

                foreach (var schoolResponse in schoolResponses) {
                    foreach (var group in schoolResponse.Groups) { //count is 3
                        var users = _userGroupRepository.GetUsers(group.Id).OfType<User>();
                        var responses = _mapper.Map<IList<UserResponse>>(users);
                        group.Users.AddRange(responses);
                    }
                }

                userResponse.Schools = schoolResponses;
                return Ok(new Response<UserResponse>(200, "okay", userResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
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
        public IActionResult ChangePassword(Guid userId, [FromBody]ResetPasswordDto password) {
            try {
                if (password is null || userId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "auth failed"));

                var user = _userRepository.Get(userId);

                if (user is null)
                    return BadRequest(new Response<object>(400, "auth failed"));

                if (!_securityService.VerifyPassword(password.OldPassword, user.PasswordHash, user.PasswordSalt))
                    return BadRequest(new Response<object>(400, "Old password is wrong"));

                var passData = _securityService.HashPassword(password.NewPassword);
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
        private readonly IMapper _mapper;
        private readonly IJoinRepository<UserGroup> _userGroupRepository;
    }
}
