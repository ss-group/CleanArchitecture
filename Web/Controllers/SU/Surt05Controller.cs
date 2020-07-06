using System.Threading.Tasks;
using Application.Features;
using Application.Features.SU.SURT05;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU
{
    [Authorize(Roles = "SURT05")]
    public class Surt05Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<SuParameter>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpGet("master")]
        public async Task<ActionResult<Master.MasterData>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}