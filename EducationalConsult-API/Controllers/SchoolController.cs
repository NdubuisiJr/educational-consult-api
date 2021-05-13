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
    [Route("/api/school/{userId}")]
    public class SchoolController : ControllerBase {

        public SchoolController(
            IMapper mapper, IRepository<User> userRepository, IRepository<Group> groupRepository,
            IJoinRepository<UserGroup> userGroupRepository, IRepository<School> schoolRepository
            ) {
            _mapper = mapper;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _userGroupRespository = userGroupRepository;
            _schoolRepository = schoolRepository;
        }

        [HttpGet("{schoolId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Response<SchoolResponse>> GetSchool(Guid userId, Guid schoolId) {
            try {
                if (userId == Guid.Empty || schoolId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "Invalid input"));

                var groups = _userGroupRespository.GetGroups(userId).OfType<Group>();
                var school = groups.Where(x => x.School.Id == schoolId)
                                   .Select(x => x.School)
                                   .FirstOrDefault();
                if (school is null)
                    return NotFound(new Response<object>(404, "we can't find what you're looking for"));

                var schoolResponse = _mapper.Map<SchoolResponse>(school);

                foreach (var group in schoolResponse.Groups) {
                    if (group.Role == Roles.Parent)
                        continue;

                    var users = _userGroupRespository.GetUsers(group.Id).OfType<User>().ToList();
                    var responses = _mapper.Map<IList<UserResponse>>(users);
                    group.Users.AddRange(responses);
                }

                return Ok(new Response<SchoolResponse>(200, "Okay", schoolResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        /// <summary>
        /// Creates a school and attaches all the groups with roles
        /// </summary>
        /// <param name="userId">The Admin's Id</param>
        /// <param name="schoolRegistration">The registration form</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Response<SchoolResponse>> CreateSchool (Guid userId, [FromBody] SchoolRegistration schoolRegistration) {
            try {
                if (userId == Guid.Empty || schoolRegistration is null)
                    return BadRequest(new Response<object>(400, "Invalid inputs"));

                var user = _userRepository.Get(userId);
                if (user is null)
                    return NotFound(new Response<object>(404, "We can't find the User"));

                var school = _mapper.Map<School>(schoolRegistration);
                var existing = _schoolRepository.GetAll()
                        .Where(x => x.OfficialEmail.Trim().ToLower() == school.OfficialEmail.Trim().ToLower() ||
                            x.OfficialPhone.Trim().ToLower() == school.OfficialPhone.Trim().ToLower())
                        .FirstOrDefault();
                if (existing is { })
                    return UnprocessableEntity(new Response<object>(422, "School already exists"));

                _schoolRepository.Add(school);

                var adminGroup = new Group() { Role = Roles.Admin, School = school };
                var teacherGroup = new Group { Role = Roles.Teacher, School = school };
                var studentGroup = new Group { Role = Roles.Parent, School = school };
                _groupRepository.AddRange(new Group[] { adminGroup, teacherGroup, studentGroup });

                _userGroupRespository.Add(new UserGroup { Group = adminGroup, User = user });

                _userGroupRespository.SaveChanges();

                var schoolResponse = _mapper.Map<SchoolResponse>(school);
                return Ok(new Response<SchoolResponse>(200, "Okay", schoolResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IJoinRepository<UserGroup> _userGroupRespository;
        private readonly IRepository<School> _schoolRepository;
    }
}
