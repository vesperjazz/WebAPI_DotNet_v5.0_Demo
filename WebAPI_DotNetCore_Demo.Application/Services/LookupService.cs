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
        private readonly IRepositoryContainer _repositoryContainer;
        public LookupService(IMapper mapper, IRepositoryContainer repositoryContainer)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repositoryContainer = repositoryContainer ?? throw new ArgumentNullException(nameof(repositoryContainer));
        }

        public async Task<GenderDto> GetGenderByIDAsync(Guid genderID, CancellationToken cancellationToken = default)
        {
            var gender = await _repositoryContainer.GenderRepository.GetAsync(genderID, cancellationToken);

            if (gender is null) { throw new NotFoundException($"Gender with ID: [{genderID}] is not found."); }

            return _mapper.Map<GenderDto>(gender);
        }

        public async Task<IEnumerable<GenderDto>> GetAllGendersAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<GenderDto>>(
                await _repositoryContainer.GenderRepository.GetAllAsync(cancellationToken));
        }

        public async Task<CountryDto> GetCountryByIDAsync(Guid countryID, CancellationToken cancellationToken = default)
        {
            var country = await _repositoryContainer.CountryRepository.GetAsync(countryID, cancellationToken);

            if(country is null) { throw new NotFoundException($"Country with ID: [{countryID}] is not found."); }

            return _mapper.Map<CountryDto>(country);
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<CountryDto>>(
                await _repositoryContainer.CountryRepository.GetAllAsync(cancellationToken));
        }

        public async Task<RoleDto> GetRoleByIDAsync(Guid roleID, CancellationToken cancellationToken = default)
        {
            var role = await _repositoryContainer.RoleRepository.GetAsync(roleID, cancellationToken);

            if(role is null) { throw new NotFoundException($"Role with ID: [{roleID}] is not found."); }

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<RoleDto>>(
                await _repositoryContainer.RoleRepository.GetAllAsync(cancellationToken));
        }
    }
}
