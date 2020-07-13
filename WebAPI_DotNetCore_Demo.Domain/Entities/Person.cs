using System;
using System.Collections.Generic;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;

namespace WebAPI_DotNetCore_Demo.Domain.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public Guid? GenderID { get; set; }
        public Gender Gender { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
