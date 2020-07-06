using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.DB.DBRT15;
using Domain.Entities.DB;

namespace Web.Controllers.DB
{
    public class Dbrt15Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpGet("detail")]
        public async Task<ActionResult<DbDegreeSub>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("master")]
        public async Task<ActionResult<DbEducationType>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        public async Task<ActionResult<long>> Post([FromBody]Create.Command model)
        {
            return await Mediator.Send(model);
        }
        [HttpPut]
        public async Task<ActionResult<long>> Put([FromBody]Edit.Command model)
        {
            return await Mediator.Send(model);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
    }
}
