using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects;

namespace WebAPI_DotNetCore_Demo.Application.Services.Interfaces
{
    public interface IPersonService
    {
        Task<PersonDto> GetPersonByIDAsync(Guid personID, CancellationToken cancellationToken = default);
        Task<IEnumerable<PersonDto>> GetAllPersonsAsync(CancellationToken cancellationToken = default);
        Task CreatePersonAsync(CreatePersonDto createPersonDto, CancellationToken cancellationToken = default);
        void UpdatePerson(UpdatePersonDto updatePersonDto);
        void UpdatePersonName(UpdatePersonNameDto updatePersonNameDto);
        Task DeletePersonByIDAsync(Guid personID, CancellationToken cancellationToken = default);
    }
}
