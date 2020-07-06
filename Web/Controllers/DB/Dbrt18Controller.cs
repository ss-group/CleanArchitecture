using Application.Features;
using Application.Features.DB.DBRT18;
using Domain.Entities.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.DB
{
    public class Dbrt18Controller: BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        
        
        [HttpGet("detail")]
        public async Task<ActionResult<DbListItem>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail/management")]
        public async Task<ActionResult<DbProgram>> Get([FromQuery]Manage.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail/management2")]
        public async Task<ActionResult<DbProgram>> Get([FromQuery]GetCode.Query model)
        {
            return Ok(await Mediator.Send(model));
        }


        [HttpPost("detail/management")]
        public async Task<IActionResult> Post([FromBody]Create.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
        
        [HttpPut("detail/management")]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpDelete("detail")]
        public async Task<IActionResult> Delete([FromQuery]Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

    }
}
