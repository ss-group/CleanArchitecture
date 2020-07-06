using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Features.SU.Menu;
using Domain.Entities.SU;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers.SU
{
    public class MenuController : BaseController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuMenu>>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
