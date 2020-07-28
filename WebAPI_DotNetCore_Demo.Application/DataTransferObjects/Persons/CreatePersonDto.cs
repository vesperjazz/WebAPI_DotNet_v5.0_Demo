using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons
{
    public class CreatePersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? GenderID { get; set; }
        public List<CreatePhoneNumberDto> PhoneNumbers { get; set; }
        public List<CreateAddressDto> Addresses { get; set; }
    }
}
