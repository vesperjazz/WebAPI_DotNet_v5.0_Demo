using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;

namespace WebAPI_DotNetCore_Demo.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PersonService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<PersonDto> GetPersonByIDAsync(Guid personID, CancellationToken cancellationToken = default)
        {
            var person = await _unitOfWork.PersonRepository.GetPersonByIDWithDetailsAsync(personID, cancellationToken);

            if (person is null) { throw new NotFoundException($"Person with ID: [{personID}] is not found."); }

            return _mapper.Map<PersonDto>(person);
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<PersonDto>>(
                await _unitOfWork.PersonRepository.GetAllPersonWithDetailsAsync(cancellationToken));
        }
    }
}
