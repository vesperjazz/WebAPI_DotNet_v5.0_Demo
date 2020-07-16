using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities;

namespace WebAPI_DotNetCore_Demo.Persistence.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        private readonly WebAPIDemoDbContext _context;
        public PersonRepository(WebAPIDemoDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private IQueryable<Person> QueryPersonWithDetails()
        {
            return _context.Persons
                .Include(p => p.Gender)
                .Include(p => p.PhoneNumbers)
                    .ThenInclude(pn => pn.Country)
                .Include(p => p.Addresses)
                    .ThenInclude(pn => pn.Country);
        }

        public async Task<IEnumerable<Person>> GetAllPersonWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await QueryPersonWithDetails().ToListAsync(cancellationToken);
        }

        public async Task<Person> GetPersonByIDWithDetailsAsync(Guid personID, CancellationToken cancellationToken = default)
        {
            return await QueryPersonWithDetails().SingleOrDefaultAsync(p => p.ID == personID, cancellationToken);
        }

        public void UpdatePersonName(Person person)
        {
            // Previous EF does not return EntityEntry<> upon attach.
            //_context.Persons.Attach(person);
            //_context.Entry(person).Property(p => p.FirstName).IsModified = true;
            //_context.Entry(person).Property(p => p.LastName).IsModified = true;

            // EFCore returns the EntityEntry<> upon attach, no need to retrieve again via _context.Entry()
            var personEntityEntry = _context.Persons.Attach(person);
            personEntityEntry.Property(p => p.FirstName).IsModified = true;
            personEntityEntry.Property(p => p.LastName).IsModified = true;
        }

        public async Task DeletePersonWithDependentsByIDAsync(Guid personID, CancellationToken cancellationToken = default)
        {
            var person = await _context.Persons
                .Include(p => p.PhoneNumbers)
                .Include(p => p.Addresses)
                .SingleAsync(p => p.ID == personID, cancellationToken);

            _context.PhoneNumbers.RemoveRange(person.PhoneNumbers);
            _context.Addresses.RemoveRange(person.Addresses);
            _context.Persons.Remove(person);
        }
    }
}
