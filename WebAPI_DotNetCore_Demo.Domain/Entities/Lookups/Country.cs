using System.Collections.Generic;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Lookups
{
    public class Country : EntityBase
    {
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public string CountryCode { get; set; }
        public int SortOrder { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
