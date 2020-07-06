using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features;
using Application.Features.DB.DBRT04;
using Domain.Entities.DB;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.DB
{
    public class Dbrt04Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));

        }

        [HttpGet("master")]
        public async Task<ActionResult<DbDistrict>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<DbDistrict>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Create.Command model)
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

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

    }
}
