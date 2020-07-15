using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Lookups;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;

namespace WebAPI_DotNetCore_Demo.Application.Services
{
    public class LookupService : ILookupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public LookupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<GenderDto> GetGenderByIDAsync(Guid genderID, CancellationToken cancellationToken = default)
        {
            var gender = await _unitOfWork.GenderRepository.GetAsync(genderID, cancellationToken);

            if (gender is null) { throw new NotFoundException($"Gender with ID: [{genderID}] is not found."); }

            return _mapper.Map<GenderDto>(gender);
        }

        public async Task<IEnumerable<GenderDto>> GetAllGendersAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<GenderDto>>(
                await _unitOfWork.GenderRepository.GetAllAsync(cancellationToken));
        }

        public async Task<CountryDto> GetCountryByIDAsync(Guid countryID, CancellationToken cancellationToken = default)
        {
            var country = await _unitOfWork.CountryRepository.GetAsync(countryID, cancellationToken);

            if(country is null) { throw new NotFoundException($"Country with ID: [{countryID}] is not found."); }

            return _mapper.Map<CountryDto>(country);
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<CountryDto>>(
                await _unitOfWork.CountryRepository.GetAllAsync(cancellationToken));
        }
    }
}
