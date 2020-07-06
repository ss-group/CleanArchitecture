using System.Threading.Tasks;
using Application.Features;
using Application.Features.SU.SURT02;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU
{
    [Authorize(Roles = "SURT02")]
    public class Surt02Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<SuProgram>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("checkDuplicate")]
        public async Task<ActionResult<SuProgram>> Get([FromQuery]CheckDuplicate.Query model)
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

        [HttpPut("copy")]
        public async Task<IActionResult> Put([FromBody]Copy.Command model)
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

        [HttpDelete("program")]
        public async Task<IActionResult> Delete(string programCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { ProgramCode = programCode, RowVersion = rowVersion });
            return NoContent();
        }

        [HttpDelete("programlabel")]
        public async Task<IActionResult> Delete(string programCode)
        {
            await Mediator.Send(new DeleteProgramLabel.Command { ProgramCode = programCode });
            return NoContent();
        }
    }
}