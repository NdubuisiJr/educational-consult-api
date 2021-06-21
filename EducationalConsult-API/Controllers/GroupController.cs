using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using static EducationalConsultAPI.Utils.Constants;
using static System.IO.File;

namespace EducationalConsultAPI.Controllers {
    [ApiController]
    [Authorize]
    [Route("/api/group")]
    public class GroupController : ControllerBase {

        public GroupController(IMapper mapper, IRepository<User> repository, 
            ICommunication communication, IRepository<Group> groupRepository, 
            IRepository<Student> studentRepository,IJoinRepository<UserGroup> userGroupRepository ) {
            _mapper = mapper;
            _userRepository = repository;
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _studentRepository = studentRepository;
            _communication = communication;
        }

        /// <summary>
        /// Accepts an invitation from a school
        /// </summary>
        /// <param name="groupId">The group the user is added</param>
        /// <param name="email">The user's email</param>
        /// <param name="IsStudent">is student flag</param>
        /// <returns></returns>
        [HttpGet("accept")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Accept([FromQuery] Guid groupId, [FromQuery]string email, [FromQuery]bool IsStudent=false) {
            try {
                if (groupId == Guid.Empty || string.IsNullOrWhiteSpace(email))
                    return BadRequest(new Response<object>(400, "Invalid input"));

                var group = _groupRepository.Get(groupId);
                if (group is null)
                    return BadRequest(new Response<object>(400, "Invalid group selected"));

                var user = IsStudent ?
                        _studentRepository.GetAll()
                        .Where(x=>x.Email.Trim().ToLower()==email.Trim().ToLower())
                        .FirstOrDefault()   
                        :
                        _userRepository.GetAll()
                        .Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower())
                        .FirstOrDefault();

                if (user is null)
                    return Redirect($"{FRONTEND_BASE_URL}/signup?count=0isStudent={IsStudent}&redirect={$"{BACKEND_BASE_URL}/api/group/accept?groupId={groupId}&email={email}&isStudent={IsStudent}"}");

                var existing = group.InvitedUsers.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());
                if (existing is null)
                    return BadRequest(new Response<object>(400, "You have accepted already"));

                group.InvitedUsers.Remove(existing);
                _groupRepository.DeleteRandom<InvitedUser>(existing.Id);
                _userGroupRepository.Add(new UserGroup() { Group = group, User = user });
                _groupRepository.Update(group);
                _userGroupRepository.SaveChanges();
                _groupRepository.SaveChanges();

                return StatusCode(200, new Response<object>(201, "Accepted successfully"));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }

        }

        /// <summary>
        /// Addeds a user to a group in a particular school
        /// </summary>
        /// <param name="userId">The admin user's Id</param>
        /// <param name="schoolId">The shool's Id</param>
        /// <param name="invitedUserRegistration">The request object</param>
        /// <returns></returns>
        [HttpPost("{userId}/{schoolId}")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddUser(Guid userId, Guid schoolId, [FromBody]InvitedUserRegistration invitedUserRegistration) {
            try {
                if (userId == Guid.Empty || schoolId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "Invalid input"));

                var groups = _userGroupRepository.GetGroups(userId).OfType<Group>();
                var result = groups.Where(x => x.School.Id == schoolId)
                                   .Select(x => new { x.School, Group = x })
                                   .FirstOrDefault();

                if (result is null)
                    return NotFound(new Response<object>(404, "The resource doesn't exist"));
                var school = result.School;
                var authorizedUserGroup = result.Group;

                if (authorizedUserGroup.Role == Roles.Parent)
                    return Unauthorized(new Response<object>(401, "You can't do this"));

                if(authorizedUserGroup.Role == Roles.Teacher && 
                    (invitedUserRegistration.Role.Trim() == Roles.Admin || invitedUserRegistration.Role.Trim() == Roles.Teacher))
                    return Unauthorized(new Response<object>(401, "You can't do this"));

                if (school is null)
                    return NotFound(new Response<object>(404, "we can't find what you're looking for"));

                var group = school.Groups.FirstOrDefault(x => x.Role == invitedUserRegistration.Role.Trim());
                if (group is null)
                    return NotFound(new Response<object>(404, "Invalid Role"));

                var exiting1 = school.Groups.SelectMany(x => x.InvitedUsers).FirstOrDefault(x => x.Email == invitedUserRegistration.Email.Trim());
                if (exiting1 is { })
                    return BadRequest(new Response<object>(400, "email is already been invited"));

                var exitingUser = _userRepository.GetAll()
                                                 .Where(x => x.Email == invitedUserRegistration.Email.Trim())
                                                 .FirstOrDefault();
                if(exitingUser is { }) {
                    var existingUserGroups = _userGroupRepository.GetGroups(exitingUser.Id).OfType<Group>();
                    var eschool = existingUserGroups.Where(x => x.School.Id == schoolId)
                                   .Select(x => x.School)
                                   .FirstOrDefault();
                    if (eschool is { })
                        return BadRequest(new Response<object>(400, "email is already in the school"));
                }

                var url = $"{BACKEND_BASE_URL}/api/group/accept?groupId={group.Id}&email={invitedUserRegistration.Email.Trim()}";
                url = invitedUserRegistration.Role.Trim() == Roles.Parent ? $"{url}&isStudent=true" : url;
                var emailPath = Helpers.GetFilePath("/invitation.html");
                var html = ReadAllText(emailPath);
                html = html.Replace("{{action_url}}", url);
                html = html.Replace("{{action_url}}", url);
                html = html.Replace("{{action_url}}", url);
                html = html.Replace("{{school_action}}", school.Name);
                html = html.Replace("{{school_action}}", school.Name);

                var sendResult = _communication.SendEmail(invitedUserRegistration.Email.Trim(), "Invitation Notice", html);
                if (!sendResult)
                    return StatusCode(500, new Response<UserResponse>(400, "Unable to send verification email. Please Try again"));

                group = _groupRepository.LoadRefrencesTypes(group);
                var invitedUser = _mapper.Map<InvitedUser>(invitedUserRegistration);
                invitedUser.Group = group;
                group.InvitedUsers.Add(invitedUser);
                _groupRepository.Update(group);
                _groupRepository.SaveChanges();
                var invitedUserResponse = _mapper.Map<InvitedUserResponse>(invitedUser);

                return Ok(new Response<InvitedUserResponse>(200, "User Invited", invitedUserResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IJoinRepository<UserGroup> _userGroupRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly ICommunication _communication;
    }
}
