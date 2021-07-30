using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Dto
{
    public class EmployeeLeaveListDto
    {
 
        public ICollection<EmployeeLeaveDto> EmployeeLeaves { get; set; }
    }
}
