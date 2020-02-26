using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SFA.DAS.ToolService.Core.Configuration;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Auth0
{
    public class Auth0ApiClient : IAuth0ApiClient
    {
        private readonly ILogger<Auth0ApiClient> _logger;
        private readonly IOptions<AuthenticationConfiguration> _configuration;

        public Auth0ApiClient(ILogger<Auth0ApiClient> logger, IOptions<AuthenticationConfiguration> configuration)
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
            var client = new RestClient($"https://{_configuration.Value.Domain}/oauth/token");

            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            var audience = $"https://{_configuration.Value.Domain}/api/v2/";
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={_configuration.Value.ManagementApiClientId}&client_secret={_configuration.Value.ManagementApiClientSecret}&audience={Uri.EscapeUriString(audience)}", ParameterType.RequestBody);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            return tokenResponse.AccessToken;
        }

        public async Task<IPagedList<Role>> GetAuth0Roles()
        {
            var token = GetToken();
            var uri = new Uri($"https://{_configuration.Value.Domain}/api/v2");
            using (var client = new ManagementApiClient(token, uri))
            {
                var request = new GetRolesRequest
                {
                    NameFilter = ""
                };

                return await client.Roles.GetAllAsync(request);
            }
        }
    }
}
