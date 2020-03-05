namespace SFA.DAS.ToolService.Core.Configuration
{
    public class AuthenticationConfiguration
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ManagementApiClientId { get; set; }
        public string ManagementApiClientSecret { get; set; }
        public string ManagementApiAudience { get; set; }
    }
}
