using System;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Enumerations;

namespace WebAPI_DotNetCore_Demo.Domain.Entities
{
    public class PhoneNumber : EntityBase
    {
        public PhoneNumberType? PhoneNumberType { get; set; }
        public string Number { get; set; }

        public Guid? CountryID { get; set; }
        public Country Country { get; set; }

        public Guid? PersonID { get; set; }
        public Person Person { get; set; }
    }
}
