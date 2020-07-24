namespace WebAPI_DotNetCore_Demo.Options
{
    public sealed class JwtSettings
    {
        public string Secret { get; set; }
        public double ExpiryInMilliseconds { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool IsValidateIssuer { get; set; }
        public bool IsValidateAudience { get; set; }
    }
}
