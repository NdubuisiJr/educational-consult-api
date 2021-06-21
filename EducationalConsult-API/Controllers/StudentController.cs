using AutoMapper;
using EducationalConsultAPI.Dtos;
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
    [Route("/api/student")]
    public class StudentController : ControllerBase {

        public StudentController(IRepository<Student> repository, IMapper mapper) {
            _studentRepository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve a student's data
        /// </summary>
        /// <param name="studentId">The student's Id</param>
        /// <returns></returns>
        [HttpGet("{studentId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Student> GetStudent(Guid studentId) {
            try {
                if (studentId == Guid.Empty)
                    return StatusCode(400, new Response<object>(400, "Invalid user Id"));

                var student = _studentRepository.Get(studentId);
                if (student is null)
                    return StatusCode(404, new Response<object>(404, "Student not found"));

                var studentResponse = _mapper.Map<StudentResponseMain>(student);

                return StatusCode(200, new Response<StudentResponseMain>(200, "okay", studentResponse));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500, "Something went wrong, It's our fault not yours"));
            }
        }

        private readonly IRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
    }
}
