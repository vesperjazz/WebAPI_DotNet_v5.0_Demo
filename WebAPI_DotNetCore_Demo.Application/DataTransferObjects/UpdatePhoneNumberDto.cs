using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects
{
    public class UpdatePhoneNumberDto
    {
        public Guid? ID { get; set; }
        public string PhoneNumberType { get; set; }
        public string Number { get; set; }
        public Guid? CountryID { get; set; }
    }
}
