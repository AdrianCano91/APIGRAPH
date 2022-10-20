using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace API_GRAPH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphController : ControllerBase
    {



        private readonly ILogger<GraphController> _logger;
        private readonly GraphServiceClient _graphClient;

        public GraphController(ILogger<GraphController> logger)
        {
            _logger = logger;
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            var tenantId = "6c617927-36b9-4ece-9910-2c95ab0dd4c8";

            // Values from app registration
            var clientId = "e4535a91-2224-40d1-ae1c-ea73261e0e76";
            var clientSecret = "cnA8Q~PPeOApd3flcSd0lyzkrBZBdNzSkmDHScbQ";

            // using Azure.Identity;
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            _graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<string>> Get()
        {

            

            var users = await _graphClient.Users
                    .Request()
                    .GetAsync();

            List<string> names = new List<string>();
            foreach (var user in users)
            {
                names.Add(user.DisplayName);
            }
            return names;

        }
    }
}