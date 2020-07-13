using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public LookupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            return (await _unitOfWork.CountryRepository.GetAllAsync(cancellationToken)).ToList();
        }

        [HttpGet("countries/{ID}")]
        public async Task<ActionResult<Country>> GetCountryByIDAsync(Guid ID, CancellationToken cancellationToken = default)
        {
            var country = await _unitOfWork.CountryRepository.GetAsync(ID, cancellationToken);

            if (country is null)
                return NotFound();
            else
                return country;
        }

        [HttpGet("genders")]
        public async Task<ActionResult<IEnumerable<Gender>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return (await _unitOfWork.GenderRepository.GetAllAsync(cancellationToken)).ToList();
        }

        [HttpGet("genders/{ID}")]
        public async Task<ActionResult<Gender>> GetGenderByIDAsync(Guid ID, CancellationToken cancellationToken = default)
        {
            var gender = await _unitOfWork.GenderRepository.GetAsync(ID, cancellationToken);

            if (gender is null)
                return NotFound();
            else
                return gender;
        }
    }
}
