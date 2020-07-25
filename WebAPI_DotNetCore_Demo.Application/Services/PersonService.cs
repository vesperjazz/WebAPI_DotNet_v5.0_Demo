using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;
using WebAPI_DotNetCore_Demo.Domain.Entities;

namespace WebAPI_DotNetCore_Demo.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryContainer _repositoryContainer;
        public PersonService(IMapper mapper, IRepositoryContainer repositoryContainer)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repositoryContainer = repositoryContainer ?? throw new ArgumentNullException(nameof(repositoryContainer));
        }

        public async Task<PersonDto> GetPersonByIDAsync(Guid personID, CancellationToken cancellationToken = default)
        {
            var person = await _repositoryContainer.PersonRepository.GetPersonByIDWithDetailsAsync(personID, cancellationToken);

            if (person is null) { throw new NotFoundException($"Person with ID: [{personID}] is not found."); }

            return _mapper.Map<PersonDto>(person);
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<PersonDto>>(
                await _repositoryContainer.PersonRepository.GetAllPersonWithDetailsAsync(cancellationToken));
        }

        public async Task CreatePersonAsync(CreatePersonDto createPersonDto, CancellationToken cancellationToken = default)
        {
            var newPerson = _mapper.Map<Person>(createPersonDto);

            await _repositoryContainer.PersonRepository.AddAsync(newPerson, cancellationToken);
        }

        public void UpdatePerson(UpdatePersonDto updatePersonDto)
        {
            var updatedPerson = _mapper.Map<Person>(updatePersonDto);
            _repositoryContainer.PersonRepository.Update(updatedPerson);
        }

        public void UpdatePersonName(UpdatePersonNameDto updatePersonNameDto)
        {
            var updatedPerson = _mapper.Map<Person>(updatePersonNameDto);
            _repositoryContainer.PersonRepository.UpdatePersonName(updatedPerson);
        }


        public async Task DeletePersonByIDAsync(Guid personID, CancellationToken cancellationToken = default)
        {
            try
            {
                await _repositoryContainer.PersonRepository.DeletePersonWithDependentsByIDAsync(personID, cancellationToken);
            }
            catch (InvalidOperationException ioex)
            {
                throw new NotFoundException($"Person with ID: [{personID}] is not found.", ioex);
            }
        }
    }
}
