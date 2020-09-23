namespace SFA.DAS.ToolService.Core.Configuration
{
    public class AuthenticationConfiguration
    {
        public GitHub GitHub { get; set; }
        public string Domain { get; set; }
        public string Realm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AdminApiClientId { get; set; }
        public string AdminApiClientSecret { get; set; }
    }

    public class GitHub
    {
        public string Organisation { get; set; }
        public string Teams { get; set; }
    }
}
