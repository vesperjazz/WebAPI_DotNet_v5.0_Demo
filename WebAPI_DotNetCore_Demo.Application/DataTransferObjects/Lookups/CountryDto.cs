using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Lookups
{
    public sealed class CountryDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public string CountryCode { get; set; }
        public int SortOrder { get; set; }
    }
}
