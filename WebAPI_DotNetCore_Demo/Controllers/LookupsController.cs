using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Lookups;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupService _lookupService;
        public LookupsController(IUnitOfWork unitOfWork, ILookupService lookupService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            return (await _lookupService.GetAllCountriesAsync(cancellationToken)).ToList();
        }

        [HttpGet("countries/{countryID}")]
        public async Task<ActionResult<CountryDto>> GetCountryByIDAsync(Guid countryID, CancellationToken cancellationToken = default)
        {
            return await _lookupService.GetCountryByIDAsync(countryID, cancellationToken);
        }

        [HttpGet("genders")]
        public async Task<ActionResult<IEnumerable<GenderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return (await _lookupService.GetAllGendersAsync(cancellationToken)).ToList();
        }

        [HttpGet("genders/{genderID}")]
        public async Task<ActionResult<GenderDto>> GetGenderByIDAsync(Guid genderID, CancellationToken cancellationToken = default)
        {
            return await _lookupService.GetGenderByIDAsync(genderID, cancellationToken);
        }
    }
}
