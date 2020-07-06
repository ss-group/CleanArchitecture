using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Features.SU.Localize;
using Domain.Entities.SU;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers.SU
{
    public class LocalizeController : BaseController
    {
        // GET: api/<controller>
        [HttpGet("{lang}")]
        public async Task<ActionResult<List.Localize>> Get([FromRoute]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{module}/{lang}")]
        public async Task<ActionResult<List.Localize>> GetByModule([FromRoute]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
