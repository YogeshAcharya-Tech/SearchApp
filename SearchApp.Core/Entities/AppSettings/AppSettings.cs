namespace SearchApp.Domain
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new();
        public JwtSetting JwtSetting { get; set; } = new();
    }
    public class ConnectionStrings
    {
        public string EmployeeDbConnection { get; set; } = string.Empty;
    }
    public class JwtSetting
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string AccessExpireMinutes { get; set; } = string.Empty;
        public string RefreshTokenExpirationMinutes { get; set; } = string.Empty;
    }
}
