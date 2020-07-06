using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features;
using Application.Features.SU.SURT07;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.SU
{
    public class Surt07Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
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

        [HttpDelete]
        public async Task<IActionResult> Delete(string passwordPolicyCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { PasswordPolicyCode = passwordPolicyCode, RowVersion = rowVersion });
            return NoContent();
        }
    }
}