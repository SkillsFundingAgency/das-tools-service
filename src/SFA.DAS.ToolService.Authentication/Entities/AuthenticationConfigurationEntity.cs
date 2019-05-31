using System;

namespace SFA.DAS.ToolService.Authentication.Entities
{
    public partial class GitHub
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ValidOrganizations { get; set; }
        public string CallbackPath { get; set; }
        public string Scope { get; set; }
        public string RedirectUrl { get; set; }

    }

    public class AuthenticationConfigurationEntity
    {
        public GitHub GitHub { get; set; }
    }
}