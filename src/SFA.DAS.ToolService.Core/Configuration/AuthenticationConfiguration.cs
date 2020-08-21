using System.Collections.Generic;

namespace SFA.DAS.ToolService.Core.Configuration
{
    public class AuthenticationConfiguration
    {
        public GitHub[] GitHub { get; set; }
        public string Domain { get; set; }
        public string Realm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ManagementApiClientId { get; set; }
        public string ManagementApiClientSecret { get; set; }
    }

    public class GitHub
    {
        public string Organisation { get; set; }
        public ValidTeam[] ValidTeams { get; set; }
    }

    public class ValidTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
