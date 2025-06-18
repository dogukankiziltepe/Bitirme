using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Entities.Medias;
using Bitirme.DAL.Entities.Courses;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace Bitirme.Controllers
{
    [ApiController]
    [Authorize]
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
            var classMedia = new ClassMedia
            {
                MediaName = fileName,
                MediaType="",
                MediaUrl =""
            };

            _classMediaService.Add(classMedia);
            return Ok(new { Message = "File uploaded successfully.", FileName = fileName });
        }


        public class MediaDTO
        {
            public IFormFile File { get; set; }
        }
    }
}