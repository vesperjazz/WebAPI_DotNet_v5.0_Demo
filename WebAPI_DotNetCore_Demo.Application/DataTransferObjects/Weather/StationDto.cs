namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather
{
    public sealed class StationDto
    {
        public string ID { get; set; }
        public string DeviceID { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
