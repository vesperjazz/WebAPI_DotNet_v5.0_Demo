using System;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Enumerations;

namespace WebAPI_DotNetCore_Demo.Domain.Entities
{
    public class Address : EntityBase
    {
        public AddressType? AddressType { get; set; }
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string ThirdLine { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Guid? CountryID { get; set; }
        public Country Country { get; set; }

        public Guid? PersonID { get; set; }
        public Person Person { get; set; }
    }
}
