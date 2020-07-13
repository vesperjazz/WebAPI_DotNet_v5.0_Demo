using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Domain.Entities;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return (await _unitOfWork.PersonRepository.GetAllPersonWithDetails(cancellationToken)).ToList();
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<Person>> GetByIDAsync(Guid ID, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.PersonRepository.GetPersonByIDWithDetails(ID, cancellationToken);

            if (person is null)
                return NotFound();
            else
                return person;
        }
    }
}
