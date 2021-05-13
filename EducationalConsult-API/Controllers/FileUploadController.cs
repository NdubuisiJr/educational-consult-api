using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Interfaces;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using static System.IO.File;

namespace EducationalConsultAPI.Controllers {

    [ApiController]
    [Route("/api/upload")]
    public class FileUploadController : ControllerBase {

        public FileUploadController(IRepository<User> repository) {
            _userRepository = repository;
        }

        /// <summary>
        /// Uploads user's profile picture
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <param name="upload">The uploaded form</param>
        /// <returns></returns>
        [HttpPost("image/{userId}")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UploadImage([FromRoute]Guid userId, [FromForm]FileUpload upload) {
            try {
                if (upload == null || upload.file == null || upload.file.Length == 0)
                    return BadRequest(new Response<object>(400, "Please select a file"));

                var user = _userRepository.Get(userId);
                if (user is null)
                    return NotFound(new Response<object>(404, "No such user"));

                if (user.ImageUrl is { }) {
                    var exitingFile = Helpers.GetFilePath(user.ImageUrl);
                    if (Exists(exitingFile))
                        Delete(exitingFile);
                }

                var fileName = $"/{DateTime.UtcNow.Ticks}_{upload.file.FileName}";
                var path = Helpers.GetFilePath(fileName);

                using (var stream = new FileStream(path, FileMode.Create)) {
                    upload.file.CopyTo(stream);
                }

                user.ImageUrl = $"{Constants.BACKEND_BASE_URL}/{fileName}";
                _userRepository.Update(user);
                _userRepository.SaveChanges();

                return Ok(new Response<object>(200, "File uploaded", new { path = user.ImageUrl }));
            }
            catch (Exception) {
                return StatusCode(500, new Response<object>(500,"Something went wrong, It's our fault not yours" ));
            }
        }

        private readonly IRepository<User> _userRepository;
    }
}
