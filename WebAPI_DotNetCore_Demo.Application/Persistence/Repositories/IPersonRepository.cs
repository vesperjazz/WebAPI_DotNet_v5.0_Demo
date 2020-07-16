using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Domain.Entities;

namespace WebAPI_DotNetCore_Demo.Application.Persistence.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person> GetPersonByIDWithDetailsAsync(Guid personID, CancellationToken cancellationToken = default);
        Task<IEnumerable<Person>> GetAllPersonWithDetailsAsync(CancellationToken cancellationToken = default);
        void UpdatePersonName(Person person);
        Task DeletePersonWithDependentsByIDAsync(Guid personID, CancellationToken cancellationToken = default);
    }
}
