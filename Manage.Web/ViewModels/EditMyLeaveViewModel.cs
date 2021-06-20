using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class EditMyLeaveViewModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUserViewModel Employee { get; set; }
        public int LeaveId { get; set; }
        public LeaveViewModel Leave { get; set; }
        public IFormFile File { get; set; }
        public string ExistingFilePath { get; set; }
    }
}
