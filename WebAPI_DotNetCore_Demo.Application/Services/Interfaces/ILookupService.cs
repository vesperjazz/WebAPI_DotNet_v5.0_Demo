using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Lookups;

namespace WebAPI_DotNetCore_Demo.Application.Services.Interfaces
{
    public interface ILookupService
    {
        Task<GenderDto> GetGenderByIDAsync(Guid genderID, CancellationToken cancellationToken = default);
        Task<IEnumerable<GenderDto>> GetAllGendersAsync(CancellationToken cancellationToken = default);
        Task<CountryDto> GetCountryByIDAsync(Guid countryID, CancellationToken cancellationToken = default);
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
        Task<RoleDto> GetRoleByIDAsync(Guid roleID, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    }
}
