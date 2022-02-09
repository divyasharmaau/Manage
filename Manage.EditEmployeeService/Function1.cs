using System;
using System.Configuration;
using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Manage.EditEmployeeService
{
    public static class EditEmployeeService
    {
        [FunctionName("EditEmployeeService")]
        public static void Run([ServiceBusTrigger("employee", "EditEmployee", Connection = "manageConnection")]
        string mySbMsg, ILogger log)
        {
            string connectionString = GetSqlAzureConnectionString("manageDbConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ManageContext>();
            optionsBuilder.UseSqlServer(connectionString);

            log.LogInformation($"C# ServiceBus topic trigger function received message: {mySbMsg}");

            try
            {
                using (var dbContext = new ManageContext(optionsBuilder.Options))
                {
                    var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(mySbMsg);
                    dbContext.Users.Update(applicationUser);
                    dbContext.SaveChanges();
                    log.LogInformation($"C# ServiceBus topic trigger function processed message {applicationUser.Id}");
                }
            }
            catch(Exception ex)
            {
                log.LogError($"{ex.Message}");
                throw;
            }
        }

        public static string GetSqlAzureConnectionString(string name)
        {
            string conStr = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(conStr)) // Azure Functions App Service naming convention
                conStr = System.Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{name}", EnvironmentVariableTarget.Process);
            return conStr;
        }
    }
}
