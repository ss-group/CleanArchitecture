using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Features.DB.DBRT11;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities.DB;

namespace Web.Controllers.DB
{
    public class Dbrt11Controller: BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Create.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpGet("detail")]
        public async Task<ActionResult<DbFac>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }



    }
}
