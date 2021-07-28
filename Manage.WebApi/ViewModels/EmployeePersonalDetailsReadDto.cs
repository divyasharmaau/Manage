using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class EmployeePersonalDetailsReadDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }

   
    }
}
