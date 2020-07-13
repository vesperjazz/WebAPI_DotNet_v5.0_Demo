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

        public async Task<IEnumerable<Person>> GetAllPersonWithDetails(CancellationToken cancellationToken = default)
        {
            return await QueryPersonWithDetails().ToListAsync(cancellationToken);
        }

        public async Task<Person> GetPersonByIDWithDetails(Guid ID, CancellationToken cancellationToken = default)
        {
            return await QueryPersonWithDetails().SingleOrDefaultAsync(p => p.ID == ID, cancellationToken);
        }
    }
}
