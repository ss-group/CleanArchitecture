using System.Threading.Tasks;
using Application.Features;
using Application.Features.SU.SURT06;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU
{
    [Authorize(Roles = "SURT06")]
    public class Surt06Controller : BaseController
    {
        public async Task<IActionResult> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("master")]
        public async Task<IActionResult> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("auto")]
        public async Task<IActionResult> Get([FromQuery]MasterAutocomplete.Query model)
        {
            return Ok(await Mediator.Send(model));
        }


        [HttpGet("master-permission-detail/{page}/{companyCode?}")]
        public async Task<IActionResult> GetDivision([FromRoute]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

       [HttpGet("permission")]
        public async Task<IActionResult> Get([FromQuery]Permission.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("employee")]
        public async Task<IActionResult> Get([FromQuery]Employee.Query model)
        {
            return Ok(await Mediator.Send(model));

        }
        [HttpPost]
        public async Task<ActionResult<long>> Post([FromBody]Create.Command model)
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
        public async Task<IActionResult> Delete([FromQuery]Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

         [HttpPost("permission")]
        public async Task<ActionResult<long>> Post([FromBody]CreatePermission.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut("permission")]
        public async Task<IActionResult> Put([FromBody]EditPermission.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
    }
}