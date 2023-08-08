using System.Net;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var kv_uri = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
            var secretClient = new SecretClient(kv_uri, new DefaultAzureCredential());
            var val = secretClient.GetSecret("db-name").Value.Value;

            //string testEnv = System.Environment.GetEnvironmentVariable("VaultUri");

            response.WriteString($"Welcome to Azure Functions! --- {val}");

            return response;
        }
    }
}
