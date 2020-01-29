using System;

namespace SFA.DAS.ToolService.Authentication.Entities
{
    public partial class Auth0
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class AuthenticationConfigurationEntity
    {
        public Auth0 Auth0 { get; set; }
    }
}
