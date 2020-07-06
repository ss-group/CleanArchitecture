using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.DB.DBRT10;
using Domain.Entities.DB;

namespace Web.Controllers.DB
{
    public class Dbrt10Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<DbEducationType>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("master")]
        public async Task<ActionResult<DbEducationType>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Create.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
    }
}
