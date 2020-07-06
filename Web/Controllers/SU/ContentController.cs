
using Application.Features.SU.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Web.Controllers.SU
{

    public class ContentController : BaseController
    {
        public async Task<IActionResult> Get([FromQuery]Find.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("serv/{id}")]
        public async Task<IActionResult> Serv([FromRoute]Find.Query query)
        {
            var content = await Mediator.Send(query);
            var fileProvider = new FileExtensionContentTypeProvider();
            // Figures out what the content type should be based on the file name.  
            if (!fileProvider.TryGetContentType(content.Name, out string contentType))
            {
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {content.Name}.");
            }
            var file = Path.Combine(@"\\127.0.0.1","content",content.Path);
            return PhysicalFile(file, contentType);
        }


        [HttpPost("image")]
        public async Task<IActionResult> Post([FromForm]UploadImage.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost("file")]
        public async Task<IActionResult> Post([FromForm]UploadFile.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}