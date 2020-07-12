using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return new List<string> { "Ivan Chin", "Aragorn Elessar", "Arwen Undomiel", "Gandalf Greyhame" };
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<string>> GetByIDAsync(Guid ID, CancellationToken cancellationToken)
        {
            return "Ivan Chin";
        }
    }
}
