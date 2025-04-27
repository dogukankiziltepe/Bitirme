using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Entities.Medias;
using Bitirme.DAL.Entities.Courses;

namespace Bitirme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassMediaController : ControllerBase
    {
        private readonly IClassMediaService _classMediaService;
        private readonly string _mediaStoragePath = "wwwroot/media";

        public ClassMediaController(IClassMediaService classMediaService)
        {
            _classMediaService = classMediaService;
        }

        /// <summary>
        /// File Yükleme Methodu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public IActionResult UploadFile([FromForm] MediaDTO model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("File is not selected.");

            if (!Directory.Exists(_mediaStoragePath))
                Directory.CreateDirectory(_mediaStoragePath);

            var fileName = Path.GetFileName(model.File.FileName);
            var filePath = Path.Combine(_mediaStoragePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }
            if (!string.IsNullOrEmpty(model.ClassId))
            {
                var classMedia = new ClassMedia
                {
                    ClassId = model.ClassId,
                    MediaName = fileName,
                    LessonId = model.LessonId
                };

                _classMediaService.Add(classMedia);
            }


            return Ok(new { Message = "File uploaded successfully.", FileName = fileName });
        }

        /// <summary>
        /// Class Id ile o Class'a ait medyalarý getirir.
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet("class/{classId}")]
        public IActionResult GetClassMedia(string classId)
        {
            var mediaList = _classMediaService.GetAll().Where(m => m.ClassId == classId).ToList();
            return Ok(mediaList);
        }
        /// <summary>
        /// fileName ile medyayý file olarak döndüren controller
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("media/{fileName}")]
        public IActionResult GetMedia(string fileName)
        {
            var filePath = Path.Combine(_mediaStoragePath, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        public class MediaDTO
        {
            public IFormFile File { get; set; }
            public string? ClassId { get; set; }
            public string? LessonId { get; set; }
        }
    }
}