using System.Threading.Tasks;
using Application.Features;
using Application.Features.SU.SURT01;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Features.SU.SURT01.ListDivision;

namespace Web.Controllers.SU
{
    [Authorize(Roles = "SURT01")]
    public class Surt01Controller : BaseController
    {
        [HttpGet("company")]
        public async Task<ActionResult<PageDto>> Get([FromQuery]ListCompany.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("division")]
        public async Task<ActionResult<Division>> Get([FromQuery]ListDivision.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("company/detail")]
        public async Task<ActionResult<SuCompany>> Get([FromQuery]DetailCompany.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("division/detail")]
        public async Task<ActionResult<SuDivision>> Get([FromQuery]DetailDivision.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("master")]
        public async Task<ActionResult<Master.MasterData>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost("company")]
        public async Task<ActionResult> Post([FromBody]CreateCompany.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost("division")]
        public async Task<ActionResult> Post([FromBody]CreateDivision.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut("company")]
        public async Task<IActionResult> Put([FromBody]EditCompany.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpPut("division")]
        public async Task<IActionResult> Put([FromBody]EditDivision.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpDelete("company")]
        public async Task<IActionResult> Delete(string companyCode, uint rowVersion)
        {
            await Mediator.Send(new DeleteCompany.Command { CompanyCode = companyCode, RowVersion = rowVersion });
            return NoContent();
        }

        [HttpDelete("division")]
        public async Task<IActionResult> Delete([FromQuery]DeleteDivision.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
    }
}