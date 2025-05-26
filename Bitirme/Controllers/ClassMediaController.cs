using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Entities.Medias;
using Bitirme.DAL.Entities.Courses;
using System.Reflection.Metadata.Ecma335;

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

   
    }
}