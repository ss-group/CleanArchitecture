using Application.Features;
using Application.Features.DB.DBRT08;
using Domain.Entities.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.DB
{
    public class Dbrt08Controller :BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));

        }

        [HttpGet("master/{page}")]
        public async Task<ActionResult<Master.MasterData>> Get([FromRoute]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<DbEmployee>> Get([FromQuery]Detail.Query model)
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

        [HttpGet("postalCode")]
        public async Task<ActionResult<PostalCodeCBB.Data>> Get([FromQuery]PostalCodeCBB.Query query)
        {
            return Ok(await Mediator.Send(query));
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


    }
}
