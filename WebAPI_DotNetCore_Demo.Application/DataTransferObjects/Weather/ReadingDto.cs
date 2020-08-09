namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather
{
    public sealed class ReadingDto
    {
        public string StationID { get; set; }

        public double Value { get; set; }
    }
}
