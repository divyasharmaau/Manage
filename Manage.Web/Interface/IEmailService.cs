using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailTo, string subject, string htmlMessage); 
    }
}
