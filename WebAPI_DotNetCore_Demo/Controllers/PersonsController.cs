using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonService _personService;
        public PersonsController(IUnitOfWork unitOfWork, IPersonService personService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return (await _personService.GetAllPersonsAsync(cancellationToken)).ToList();
        }

        [HttpGet("{personID}")]
        public async Task<ActionResult<PersonDto>> GetByIDAsync(Guid personID, CancellationToken cancellationToken)
        {
            return await _personService.GetPersonByIDAsync(personID, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonAsync([FromBody] CreatePersonDto createPersonDto, CancellationToken cancellationToken)
        {
            await _personService.CreatePersonAsync(createPersonDto, cancellationToken);
            await _unitOfWork.CompleteWithAuditAsync(cancellationToken);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonAsync([FromBody] UpdatePersonDto updatePersonDto, CancellationToken cancellationToken)
        {
            _personService.UpdatePerson(updatePersonDto);
            await _unitOfWork.CompleteWithAuditAsync(cancellationToken);

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePersonBirthdayAsync([FromBody] UpdatePersonNameDto updatePersonNameDto, CancellationToken cancellationToken)
        {
            _personService.UpdatePersonName(updatePersonNameDto);
            await _unitOfWork.CompleteWithAuditAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{personID}")]
        public async Task<IActionResult> DeletePersonAsync(Guid personID, CancellationToken cancellationToken)
        {
            await _personService.DeletePersonByIDAsync(personID, cancellationToken);

            // No need for audit columns changes for hard delete.
            await _unitOfWork.CompleteAsync(cancellationToken);

            return NoContent();
        }
    }
}
