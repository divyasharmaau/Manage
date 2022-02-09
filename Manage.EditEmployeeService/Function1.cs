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
            var optionsBuilder = new DbContextOptionsBuilder<ManageContext>();
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["manageDbConnection"].ConnectionString);

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

        private static string GetEnvironmentVariable(string name)
        {
            return name + ": " +
                System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
