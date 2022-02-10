using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manage.EditEmployeeService
{
    public static class EditEmployeeService
    {
        [FunctionName("EditEmployeeService")]
        public static async Task RunAsync([ServiceBusTrigger("employee", "EditEmployee", Connection = "manageConnection")]
        string mySbMsg, ILogger log)
        {         
            log.LogInformation($"C# ServiceBus topic trigger function received message: {mySbMsg}");

            await UpdateWebAiAsync(mySbMsg, log);     
        }

        private static async Task UpdateWebAiAsync(string mySbMsg, ILogger log)
        {

            TokenResponse tokenResponse;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44330");
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                var body = new TokenRequest
                {
                    username = "demoasadmn@gmail.com",
                    client_id = "manage",
                    grant_type = "password",
                    password = "Password1!",
                    refresh_token = "ca705f8f4d9540978560529b796cc54b",
                    role_name = "Administrator"
                };

                var bodyJson = JsonConvert.SerializeObject(body);
                var content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json");


                var response = await client.PostAsync("api/account/Auth/", content);

                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>
                    (await response.Content.ReadAsStringAsync());

                log.LogInformation($"tokenResponse: {tokenResponse.token}");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44330");
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");


                var bodyJson = JsonConvert.SerializeObject(mySbMsg);
                var content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json");

                var id = ((JObject)JsonConvert.DeserializeObject(mySbMsg))["Id"].Value<string>();

                var url = $"api/Employee/UpdateOfficialDetailsByAdmin/{id}";

                var response = await client.PutAsync(url, content);


                log.LogInformation($"tokenResponse: {await response.Content.ReadAsStringAsync()}");
            }

        }
    }
}
