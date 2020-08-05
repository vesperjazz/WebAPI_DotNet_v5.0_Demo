namespace WebAPI_DotNetCore_Demo.Infrastructure.Options
{
    public sealed class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int TimeoutMs { get; set; }
    }
}
