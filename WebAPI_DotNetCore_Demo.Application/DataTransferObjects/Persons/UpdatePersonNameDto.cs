using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons
{
    public class UpdatePersonNameDto
    {
        public Guid? ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
