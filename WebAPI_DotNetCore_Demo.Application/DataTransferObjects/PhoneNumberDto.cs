namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects
{
    public sealed class PhoneNumberDto
    {
        public string PhoneNumberType { get; set; }
        public string Number { get; set; }
        public string CountryCode { get; set; }
    }
}
