using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons
{
    public class UpdatePersonDto
    {
        public Guid? ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? GenderID { get; set; }
        public List<UpdatePhoneNumberDto> PhoneNumbers { get; set; }
        public List<UpdateAddressDto> Addresses { get; set; }
    }
}
