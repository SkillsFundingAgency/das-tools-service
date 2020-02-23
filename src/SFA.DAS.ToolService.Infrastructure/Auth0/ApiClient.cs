using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Microsoft.Extensions.Options;
using SFA.DAS.ToolService.Core.Configuration;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using RestSharp;
using Newtonsoft.Json;

namespace SFA.DAS.ToolService.Infrastructure.Auth0
{
    public class ApiClient : IApiClient
    {

        private readonly ILogger<ApiClient> logger;
        private readonly IOptions<AuthenticationConfiguration> configuration;

        public ApiClient(ILogger<ApiClient> _logger, IOptions<AuthenticationConfiguration> _configuration)
        {
            logger = _logger;
            configuration = _configuration;
        }

        public class TokenResponse
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
            var client = new RestClient($"https://{configuration.Value.Domain}/oauth/token");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            var audience = $"https://{configuration.Value.Domain}/api/v2/";
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={configuration.Value.ManagementApiClientId}&client_secret={configuration.Value.ManagementApiClientSecret}&audience={Uri.EscapeUriString(audience)}", ParameterType.RequestBody);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            return tokenResponse.AccessToken;
        }

        public async Task<IPagedList<Role>> GetRoles()
        {
            var token = GetToken();
            var uri = new Uri($"https://{configuration.Value.Domain}/api/v2");
            var client = new ManagementApiClient(token, uri);

            var request = new GetRolesRequest
            {
                NameFilter = ""
            };

            return await client.Roles.GetAllAsync(request);
        }
    }
}
