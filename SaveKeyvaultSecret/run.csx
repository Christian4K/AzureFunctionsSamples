#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    var keyVaultUri = GetEnvironmentVariable("KeyVaultUrl");
    var keyVaultSecretName = GetEnvironmentVariable("KeyVaultSecretName");

    log.LogInformation("Keyvault: " + keyVaultUri);
    log.LogInformation("SecretName: " + keyVaultSecretName);

    string token = req.Query["token"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    token = token ?? data?.token;

    var client = new SecretClient(vaultUri: new Uri(keyVaultUri), credential: new DefaultAzureCredential());   

    if (!string.IsNullOrEmpty(token))
    {        
        try
        {
            client.SetSecret(keyVaultSecretName, token);
            return new OkObjectResult("Function executed");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            
            return new ObjectResult(ex.Message)
            {
                StatusCode = 500
            };
        }
    }

    return new BadRequestObjectResult("No Token given");
}

public static string GetEnvironmentVariable(string name)
{
    return 
        System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
}