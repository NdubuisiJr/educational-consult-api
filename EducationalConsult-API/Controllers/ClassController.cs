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
    [Route("/api/class/{userId}")]
    public class ClassController : ControllerBase{

        public ClassController(IMapper mapper, IRepository<Class> classRepository, 
            IJoinRepository<UserGroup> userGroupRepository, IRepository<User> userRepository, 
            IRepository<School> schoolRepository) {
            _mapper = mapper;
            _classRepository = classRepository;
            _userRepository = userRepository;
            _schoolRepository = schoolRepository;
            _userGroupRepository = userGroupRepository;
        }

        /// <summary>
        /// Returns a class witht the given Id
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <param name="classId">class id</param>
        /// <returns></returns>
        [HttpGet("{classId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ClassResponse> GetClass(Guid userId, Guid classId) {
            try {
                if (userId == Guid.Empty || classId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "Invalid input"));

                var user = _userRepository.Get(userId);
                if (user is null)
                    return NotFound(new Response<object>(404, "User not found"));

                var class_ = _classRepository.Get(classId);
                if(class_ is null)
                    return NotFound(new Response<object>(404, "User not found"));

                var role = string.Empty;
                var students = new List<Student>();
                foreach (var group in class_.School.Groups) {
                    var users = _userGroupRepository.GetUsers(group.Id).OfType<User>().ToList();
                    if(role == string.Empty)
                        role = users.FirstOrDefault(x => x.Id == userId) is { } ? group.Role : string.Empty;
                    if (group.Role == Roles.Parent)
                        students.AddRange(users.OfType<Student>());
                }
                var classResponse = _mapper.Map<ClassResponse>(class_);
                var studentResponse = _mapper.Map<IEnumerable<StudentResponse>>(students);
                return StatusCode(200, new Response<object>(200, "okay", new { role, studentResponse, classResponse }));

            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        /// <summary>
        /// Creates a new class. 
        /// </summary>
        /// <param name="userId">The admin Teacher's Id</param>
        /// <param name="schoolId">The school Id</param>
        /// <param name="name">The class name</param>
        /// <returns></returns>
        [HttpPost("{schoolId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Response<ClassResponse>> CreateClass(Guid userId, Guid schoolId, [FromQuery] string name) {
            try {
                if (userId == Guid.Empty)
                    return BadRequest(new Response<object>(400, "Invalid input"));

                var user = _userRepository.Get(userId);
                if (user is null)
                    return NotFound(new Response<object>(404, "User not found"));

                var school = _schoolRepository.Get(schoolId);
                if (school is null)
                    return NotFound(new Response<object>(404, "School is not found"));

                var teacherGroup = school.Groups.FirstOrDefault(x => x.Role == Roles.Teacher);

                var teacher = _userGroupRepository.GetGroups(userId).FirstOrDefault(x => x.Id == teacherGroup.Id);

                if (teacher is null)
                    return Unauthorized(new Response<object>(401, "Only teachers can create classes"));

                var _class = new Class() {
                    Name = name,
                    User = user,
                    School = school,
                };

                _classRepository.Add(_class);
                _classRepository.SaveChanges();

                var classResponse = _mapper.Map<ClassResponse>(_class);

                return Ok(new Response<ClassResponse>(200, "Class created", classResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        private readonly IMapper _mapper;
        private readonly IRepository<Class> _classRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<School> _schoolRepository;
        private readonly IJoinRepository<UserGroup> _userGroupRepository;
    }
}
