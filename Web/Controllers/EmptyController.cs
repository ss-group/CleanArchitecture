using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [AllowAnonymous]
    public class EmptyController : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Index()
        {
            return NotFound("SSRU Smart student Server started,request to api/{controller}/{action}");
        }

    }
}
