using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects
{
    public class CreateAddressDto
    {
        public string AddressType { get; set; }
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string ThirdLine { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Guid? CountryID { get; set; }
    }
}
