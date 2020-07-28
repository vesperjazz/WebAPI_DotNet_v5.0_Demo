using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons
{
    public sealed class PersonDto
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string GenderName { get; set; }
        public List<PhoneNumberDto> PhoneNumbers { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
