using System;

namespace SFA.DAS.ToolService.Core.Entities
{
    public class GitHub
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ValidOrganizations { get; set; }
        public string CallbackPath { get; set; }
        public string Scope { get; set; }
    }

    public class Authentication
    {
        public GitHub GitHub { get; set; }
    }

    public class AuthenticationConfiguration
    {
        public Authentication Authentication { get; set; }
    }
}