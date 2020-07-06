using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Features.SU.Personal;
namespace Web.Controllers.SU
{
    public class PersonalController : BaseController
    {
        public async Task<IActionResult> Get([FromQuery]Detail.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("organization")]
        public async Task<IActionResult> Get([FromQuery]Organization.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}