using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.SU;
using Application.Features.SU.Parameter;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers.SU
{
    public class ParameterController : BaseController
    {
        // GET: api/<controller>
        [HttpGet("{group}/{code?}")]
        public async Task<IActionResult> Get([FromRoute]Detail.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
