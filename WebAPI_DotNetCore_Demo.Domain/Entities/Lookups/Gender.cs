using System.Collections;
using System.Collections.Generic;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Lookups
{
    public class Gender : EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}
