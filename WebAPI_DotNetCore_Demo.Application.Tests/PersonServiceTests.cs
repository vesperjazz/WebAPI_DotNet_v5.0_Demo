using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.MappingProfiles;
using WebAPI_DotNetCore_Demo.Application.Services;
using WebAPI_DotNetCore_Demo.Domain.Entities;
using WebAPI_DotNetCore_Demo.Domain.Enumerations;
using Xunit;

namespace WebAPI_DotNetCore_Demo.Application.Tests
{
    public class PersonServiceTests : DbContextServiceTestBase
    {
        private readonly Mapper _mapper;
        private readonly PersonService _personServiceSUT;

        public PersonServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new PersonProfileMapping())));
            _personServiceSUT = new PersonService(_mapper, _unitOfWork);

            // https://www.fakeaddressgenerator.com/
            _context.Persons.Add(new Person
            {
                ID = new Guid("A4FD9D37-D870-49AE-9662-000897A43232"),
                FirstName = "Blake",
                LastName = "Warner",
                DateOfBirth = new DateTime(1992, 05, 07),
                GenderID = new Guid("A74E7E41-DEA4-4C67-ADD6-785735717CDC"),
                PhoneNumbers = new List<PhoneNumber> 
                {
                    new PhoneNumber 
                    { 
                        PhoneNumberType = PhoneNumberType.Mobile,
                        Number = "2025550138",
                        CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                    },
                    new PhoneNumber
                    {
                        PhoneNumberType = PhoneNumberType.Home,
                        Number = "2025550174",
                        CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                    }
                },
                Addresses = new List<Address> 
                {
                    new Address
                    {
                        AddressType = AddressType.Home,
                        FirstLine = "3096 Grant View Drive",
                        PostCode = "53151",
                        City = "New Berlin",
                        State = "Wisconsin",
                        CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                    }
                }
            });

            _context.Persons.Add(new Person
            {
                ID = new Guid("0E6B5E3E-AFB0-4851-9A82-000DA6824285"),
                FirstName = "Amanda",
                LastName = "Sellers",
                DateOfBirth = new DateTime(1960, 11, 24),
                GenderID = new Guid("1AF6209B-5CF1-408E-B89B-4BDF8C302C09"),
                PhoneNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber
                    {
                        PhoneNumberType = PhoneNumberType.Mobile,
                        Number = "3104887364",
                        CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                    }
                },
            });

            _context.Persons.Add(new Person
            {
                ID = new Guid("F0E7233E-6F6B-450A-9B6C-0013C7686E7B"),
                FirstName = "Brittany",
                LastName = "Price",
                DateOfBirth = new DateTime(1987, 12, 26),
                GenderID = new Guid("1AF6209B-5CF1-408E-B89B-4BDF8C302C09"),
                Addresses = new List<Address>
                {
                    new Address
                    {
                        AddressType = AddressType.Home,
                        FirstLine = "1841 Michael Street",
                        PostCode = "77063",
                        City = "Houston",
                        State = "Texas",
                        CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                    }
                }
            });

            _context.SaveChanges();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        [Theory]
        [InlineData("D2D8A0A1-2525-4A30-8437-7BA86B758A39")]
        [InlineData("8FFD8119-84F8-47CC-B9A4-0F9CCB1F6BD2")]
        [InlineData("A6F3F880-D173-4391-9C55-0C4C11F14D35")]
        public async Task GetPersonByIDAsync_InvalidPersonID_ThrowsNotFoundException(string invalidPersonIDString)
        {
            // Arrange
            var invalidPersonID = new Guid(invalidPersonIDString);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                // Act
                await _personServiceSUT.GetPersonByIDAsync(invalidPersonID);
            });
        }

        [Theory]
        [InlineData("A4FD9D37-D870-49AE-9662-000897A43232")]
        [InlineData("0E6B5E3E-AFB0-4851-9A82-000DA6824285")]
        [InlineData("F0E7233E-6F6B-450A-9B6C-0013C7686E7B")]
        public async Task GetPersonByIDAsync_ValidPersonID_ReturnsUserDto(string validPersonIDString)
        {
            // Arrange
            var validPersonID = new Guid(validPersonIDString);

            // Act
            var personDto = await _personServiceSUT.GetPersonByIDAsync(validPersonID);

            // Assert
            Assert.NotNull(personDto);
            Assert.NotEqual(Guid.Empty, personDto.ID);
            Assert.Equal(validPersonID, personDto.ID);
            Assert.False(string.IsNullOrWhiteSpace(personDto.FirstName));
            Assert.False(string.IsNullOrWhiteSpace(personDto.LastName));
            Assert.All(personDto.PhoneNumbers, (phoneNumber) => 
            {
                Assert.False(string.IsNullOrWhiteSpace(phoneNumber.PhoneNumberType));
                Assert.False(string.IsNullOrWhiteSpace(phoneNumber.Number));
                Assert.False(string.IsNullOrWhiteSpace(phoneNumber.CountryCode));
            });
            Assert.All(personDto.Addresses, (address) =>
            {
                Assert.False(string.IsNullOrWhiteSpace(address.AddressType));
                Assert.False(string.IsNullOrWhiteSpace(address.FirstLine));
                Assert.False(string.IsNullOrWhiteSpace(address.PostCode));
                Assert.False(string.IsNullOrWhiteSpace(address.City));
                Assert.False(string.IsNullOrWhiteSpace(address.State));
                Assert.False(string.IsNullOrWhiteSpace(address.CountryName));
                Assert.False(string.IsNullOrWhiteSpace(address.CountryISOCode));
            });
        }

        [Fact]
        public async Task GetAllPersonsAsync_ReturnsAllPersonDtos()
        {
            // Act
            var personDtos = await _personServiceSUT.GetAllPersonsAsync();

            // Arrange
            Assert.NotNull(personDtos);
            Assert.NotEmpty(personDtos);
            Assert.Equal(3, personDtos.Count());
            Assert.All(personDtos, (personDto) =>
            {
                Assert.NotNull(personDto);
                Assert.NotEqual(Guid.Empty, personDto.ID);
                Assert.False(string.IsNullOrWhiteSpace(personDto.FirstName));
                Assert.False(string.IsNullOrWhiteSpace(personDto.LastName));
                Assert.All(personDto.PhoneNumbers, (phoneNumber) =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(phoneNumber.PhoneNumberType));
                    Assert.False(string.IsNullOrWhiteSpace(phoneNumber.Number));
                    Assert.False(string.IsNullOrWhiteSpace(phoneNumber.CountryCode));
                });
                Assert.All(personDto.Addresses, (address) =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(address.AddressType));
                    Assert.False(string.IsNullOrWhiteSpace(address.FirstLine));
                    Assert.False(string.IsNullOrWhiteSpace(address.PostCode));
                    Assert.False(string.IsNullOrWhiteSpace(address.City));
                    Assert.False(string.IsNullOrWhiteSpace(address.State));
                    Assert.False(string.IsNullOrWhiteSpace(address.CountryName));
                    Assert.False(string.IsNullOrWhiteSpace(address.CountryISOCode));
                });
            });
        }

        public static List<Guid> ExistingPersonIDs => new List<Guid>
        {
            new Guid("A4FD9D37-D870-49AE-9662-000897A43232"),
            new Guid("0E6B5E3E-AFB0-4851-9A82-000DA6824285"),
            new Guid("F0E7233E-6F6B-450A-9B6C-0013C7686E7B")
        };

        public static IEnumerable<object[]> ValidPersonData => new List<object[]>
        {
            new object[] 
            { 
                new CreatePersonDto
                {
                    FirstName = "Yvonne",
                    LastName = "Pena",
                    DateOfBirth = new DateTime(1967, 5, 20),
                    GenderID = new Guid("1AF6209B-5CF1-408E-B89B-4BDF8C302C09"),
                    PhoneNumbers = new List<CreatePhoneNumberDto>
                    {
                        new CreatePhoneNumberDto
                        {
                            PhoneNumberType = "Mobile",
                            Number = "3102660306",
                            CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                        },
                        new CreatePhoneNumberDto
                        {
                            PhoneNumberType = "Home",
                            Number = "6502616477",
                            CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                        }
                    },
                    Addresses = new List<CreateAddressDto>
                    {
                        new CreateAddressDto
                        {
                            AddressType = "Home",
                            FirstLine = "2777 Ella Street",
                            PostCode = "94063",
                            City = "Redwood City",
                            State = "California",
                            CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                        }
                    }
                }
            },
            new object[]
            {
                new CreatePersonDto
                {
                    FirstName = "Dawn",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(1991, 01, 12),
                    GenderID = new Guid("A74E7E41-DEA4-4C67-ADD6-785735717CDC"),
                    PhoneNumbers = new List<CreatePhoneNumberDto>
                    {
                        new CreatePhoneNumberDto
                        {
                            PhoneNumberType = "Mobile",
                            Number = "3102660306",
                            CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                        }
                    },
                }
            },
            new object[]
            {
                new CreatePersonDto
                {
                    FirstName = "Brian",
                    LastName = "Gaskins",
                    DateOfBirth = new DateTime(1970, 03, 06),
                    GenderID = new Guid("A74E7E41-DEA4-4C67-ADD6-785735717CDC"),
                    Addresses = new List<CreateAddressDto>
                    {
                        new CreateAddressDto
                        {
                            AddressType = "Office",
                            FirstLine = "3248 Courtright Street",
                            PostCode = "58230",
                            City = "Finley",
                            State = "North Dakota",
                            CountryID = new Guid("7291A7AF-5A82-4685-AE05-AFDEEC25A228")
                        }
                    }
                }
            },
        };

        [Theory]
        [MemberData(nameof(ValidPersonData))]
        public async Task CreatePersonAsync_NewPerson_PersonCreated(CreatePersonDto createPersonDto)
        {
            // Arrange

            // Act
            await _personServiceSUT.CreatePersonAsync(createPersonDto);
            await _unitOfWork.CompleteAsync();

            // Assert
            var domainPerson = await _context.Persons
                .Where(p => !ExistingPersonIDs.Contains(p.ID.Value))
                .SingleOrDefaultAsync();

            Assert.NotNull(domainPerson);
            Assert.NotNull(domainPerson.ID);
            Assert.NotEqual(Guid.Empty, domainPerson.ID);
            Assert.False(string.IsNullOrWhiteSpace(domainPerson.FirstName));
            Assert.False(string.IsNullOrWhiteSpace(domainPerson.LastName));
            Assert.All(domainPerson.PhoneNumbers, (phoneNumber) =>
            {
                Assert.True(phoneNumber.CountryID.HasValue);
                Assert.True(phoneNumber.PhoneNumberType.HasValue);
                Assert.False(string.IsNullOrWhiteSpace(phoneNumber.Number));
            });
            Assert.All(domainPerson.Addresses, (address) =>
            {
                Assert.True(address.CountryID.HasValue);
                Assert.True(address.AddressType.HasValue);
                Assert.False(string.IsNullOrWhiteSpace(address.FirstLine));
                Assert.False(string.IsNullOrWhiteSpace(address.PostCode));
                Assert.False(string.IsNullOrWhiteSpace(address.City));
                Assert.False(string.IsNullOrWhiteSpace(address.State));
            });
        }

        [Theory]
        [InlineData("7B7A40C2-DE77-4BBD-9BE2-0006E829E69F")]
        [InlineData("CE903176-28F5-4309-9B21-000C3757C73E")]
        [InlineData("EC0B96BF-0836-4788-97BF-E75274ECF73D")]
        public async Task DeletePersonByIDAsync_InvalidPersonID_ThrowsNotFoundException(string invalidPersonIDString)
        {
            // Arrange
            var invalidPersonID = new Guid(invalidPersonIDString);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                // Act
                await _personServiceSUT.DeletePersonByIDAsync(invalidPersonID);
                await _unitOfWork.CompleteAsync();
            });
        }

        [Theory]
        [InlineData("A4FD9D37-D870-49AE-9662-000897A43232")]
        [InlineData("0E6B5E3E-AFB0-4851-9A82-000DA6824285")]
        [InlineData("F0E7233E-6F6B-450A-9B6C-0013C7686E7B")]
        public async Task DeletePersonByIDAsync_ValidPersonID_PersonDeleted(string validPersonIDString)
        {
            // Arrange
            var validPersonID = new Guid(validPersonIDString);

            // Act
            await _personServiceSUT.DeletePersonByIDAsync(validPersonID);
            await _unitOfWork.CompleteAsync();

            // Assert
            var domainPerson = await _context.Persons
                .SingleOrDefaultAsync(p => p.ID == validPersonID);

            Assert.Null(domainPerson);
        }
    }
}
