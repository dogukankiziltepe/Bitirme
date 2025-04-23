using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Entities.Medias;

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

        [HttpPost("upload")]
        public IActionResult UploadFile([FromForm] IFormFile file, [FromForm] string classId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is not selected.");

            if (!Directory.Exists(_mediaStoragePath))
                Directory.CreateDirectory(_mediaStoragePath);

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_mediaStoragePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var classMedia = new ClassMedia
            {
                ClassId = classId,
                MediaName = fileName
            };

            _classMediaService.Add(classMedia);

            return Ok(new { Message = "File uploaded successfully.", FileName = fileName });
        }

        [HttpGet("class/{classId}")]
        public IActionResult GetClassMedia(string classId)
        {
            var mediaList = _classMediaService.GetAll().Where(m => m.ClassId == classId).ToList();
            return Ok(mediaList);
        }

        [HttpGet("media/{fileName}")]
        public IActionResult GetMedia(string fileName)
        {
            var filePath = Path.Combine(_mediaStoragePath, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}