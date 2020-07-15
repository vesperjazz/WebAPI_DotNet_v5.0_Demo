using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Lookups
{
    public sealed class GenderDto
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
