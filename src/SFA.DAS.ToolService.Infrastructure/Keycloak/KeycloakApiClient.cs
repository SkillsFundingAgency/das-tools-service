using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SFA.DAS.ToolService.Core.Configuration;
using SFA.DAS.ToolService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Keycloak
{
    public class KeycloakApiClient : IKeycloakApiClient
    {
        private readonly ILogger<KeycloakApiClient> _logger;
        private readonly IOptions<AuthenticationConfiguration> _configuration;

        public KeycloakApiClient(ILogger<KeycloakApiClient> logger, IOptions<AuthenticationConfiguration> configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in")]
            public long ExpiresIn { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }
        }

        private string GetToken()
        {
            var client = new RestClient($"https://{_configuration.Value.Domain}/realms/{_configuration.Value.Realm}/protocol/openid-connect/token");

            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={_configuration.Value.Realm}&client_secret={_configuration.Value.ManagementApiClientSecret}", ParameterType.RequestBody);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            return tokenResponse.AccessToken;
        }

        public async Task<IList<ExternalRole>> GetKeycloakRoles()
        {
            var client = new RestClient($"https://{_configuration.Value.Domain}/admin/realms/{_configuration.Value.Realm}/roles");

            var request = new RestRequest(Method.GET);

            var token = GetToken();

            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.ExecuteAsync(request);

            var roles = JsonConvert.DeserializeObject<IList<ExternalRole>>(response.Content);

            return roles;
        }
    }
}
