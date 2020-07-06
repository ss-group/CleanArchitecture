using Application.Features;
using Application.Features.DB.DBRT06;
using Domain.Entities.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.DB
{
    public class Dbrt06Controller :BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));

        }
        [HttpGet("master")]
        public async Task<ActionResult<DbPostalCode>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("district")]
        public async Task<ActionResult<DistrictCBB.Data>> Get([FromQuery]DistrictCBB.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("subDistrict")]
        public async Task<ActionResult<SubDistrictCBB.Data>> Get([FromQuery]SubDistrictCBB.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<DbDistrict>> Get([FromQuery]Detail.Query model)
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
