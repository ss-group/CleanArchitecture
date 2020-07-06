using System.Threading.Tasks;
using Application.Features;
using Application.Features.SU.SURT04;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Features.SU.SURT04.Detail;

namespace Web.Controllers.SU
{
    [Authorize(Roles = "SURT04")]
    public class Surt04Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<SuProfileVm>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("master")]
        public async Task<ActionResult<PageDto>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string profileCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { ProfileCode = profileCode, RowVersion = rowVersion });
            return NoContent();
        }

        [HttpPost("copy")]
        public async Task<ActionResult> Post([FromBody]Copy.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}