using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons
{
    public class CreatePhoneNumberDto
    {
        public string PhoneNumberType { get; set; }
        public string Number { get; set; }
        public Guid? CountryID { get; set; }
    }
}
